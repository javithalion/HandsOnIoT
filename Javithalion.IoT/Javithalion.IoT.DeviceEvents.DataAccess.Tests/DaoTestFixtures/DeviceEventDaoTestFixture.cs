using Javithalion.IoT.DeviceEvents.DataAccess.DAOs;
using Javithalion.IoT.DeviceEvents.Domain.Entities;
using MongoDB.Driver;
using Moq;
using System;
using System.Linq;
using MongoDB.Bson;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Javithalion.IoT.DeviceEvents.DataAccess.Tests.DaoTestFixtures.Support;
using Newtonsoft.Json.Linq;
using MongoDB.Bson.IO;

namespace Javithalion.IoT.DeviceEvents.DataAccess.Tests.DaoTestFixtures
{
    public class DeviceEventDaoTestFixture
    {
        //REMARK :: Dao internals are not tested since we are not supposed to unit test third party frameworks/drivers. 
        //          Integration tests are supposed to use them

        private const string CollectionName = "DeviceEventCollection";

        [Fact(DisplayName = "FindAll_CallsGetCollection")]
        [Trait("Category", "DeviceEvents.DataAccess.DeviceEventDao")]
        public async Task FindAll_CallsGetCollection()
        {
            var mockedMongoDbCollection = new Mock<IMongoCollection<DeviceEvent>>();
            var mongoDatabaseMock = new Mock<IMongoDatabase>(MockBehavior.Strict);
            mongoDatabaseMock.Setup(m => m.GetCollection<DeviceEvent>(CollectionName, null)).Returns(() => { return mockedMongoDbCollection.Object; });

            var dao = new DeviceEventDao(mongoDatabaseMock.Object);

            var result = dao.AllDeviceEvents();

            mongoDatabaseMock.Verify(m => m.GetCollection<DeviceEvent>(CollectionName, null), Times.Once());
        }

        [Fact(DisplayName = "Insert_CallsInsertOne")]
        [Trait("Category", "DeviceEvents.DataAccess.DeviceEventDao")]
        public async Task Insert_CallsInsertOne()
        {
            var deviceEvent = DeviceEvent.NewStartUpEvent(Guid.NewGuid());
            DeviceEvent insertedItem = null;

            var mockedMongoDbCollection = new Mock<IMongoCollection<DeviceEvent>>(MockBehavior.Strict);
            var mongoDatabaseMock = new Mock<IMongoDatabase>(MockBehavior.Strict);

            mockedMongoDbCollection.Setup(m => m.InsertOneAsync(deviceEvent, It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()))
                            .Callback((DeviceEvent item, InsertOneOptions options, CancellationToken cancellationToken) => { insertedItem = item; })
                            .Returns(Task.FromResult(new object()));

            mongoDatabaseMock.Setup(m => m.GetCollection<DeviceEvent>(CollectionName, null)).Returns(() => { return mockedMongoDbCollection.Object; });

            var dao = new DeviceEventDao(mongoDatabaseMock.Object);

            await dao.InsertAsync(deviceEvent);

            mockedMongoDbCollection.Verify(m => m.InsertOneAsync(deviceEvent, It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()), Times.Once());
            mongoDatabaseMock.Verify(m => m.GetCollection<DeviceEvent>(CollectionName, null), Times.Once());

            Assert.Equal(deviceEvent, insertedItem);            
        }

        [Fact(DisplayName = "UpdateNoDetails_CallsUpdateOne")]
        [Trait("Category", "DeviceEvents.DataAccess.DeviceEventDao")]
        public async Task UpdateNoDetails_CallsUpdateOne()
        {
            var deviceId = Guid.NewGuid();
            var deviceEvent = DeviceEvent.NewStartUpEvent(deviceId);
            UpdateDefinition<DeviceEvent> updateDefinition = null;
            FilterDefinition<DeviceEvent> filterDefinition = null;

            var mockedMongoDbCollection = new Mock<IMongoCollection<DeviceEvent>>(MockBehavior.Strict);
            var mongoDatabaseMock = new Mock<IMongoDatabase>(MockBehavior.Strict);
            var updateResultMock = new Mock<UpdateResult>();

            mockedMongoDbCollection.Setup(m => m.UpdateOneAsync(It.IsAny<FilterDefinition<DeviceEvent>>(), It.IsAny<UpdateDefinition<DeviceEvent>>(), It.IsAny<UpdateOptions>(), It.IsAny<CancellationToken>()))
                            .Callback((FilterDefinition<DeviceEvent> filter, UpdateDefinition<DeviceEvent> update, UpdateOptions options, CancellationToken cancellationToken) =>
                                        {
                                            filterDefinition = filter;
                                            updateDefinition = update;
                                        })
                            .Returns(Task.FromResult(updateResultMock.Object));

            mongoDatabaseMock.Setup(m => m.GetCollection<DeviceEvent>(CollectionName, null)).Returns(() => { return mockedMongoDbCollection.Object; });

            var dao = new DeviceEventDao(mongoDatabaseMock.Object);
            await dao.UpdateAsync(deviceEvent);

            mockedMongoDbCollection.Verify(m => m.UpdateOneAsync(It.IsAny<FilterDefinition<DeviceEvent>>(), It.IsAny<UpdateDefinition<DeviceEvent>>(), It.IsAny<UpdateOptions>(), It.IsAny<CancellationToken>()), Times.Once());
            mongoDatabaseMock.Verify(m => m.GetCollection<DeviceEvent>(CollectionName, null), Times.Once());

            var filterDefinitionBson = filterDefinition.RenderToBsonDocument();
            var updateDefinitionBson = updateDefinition.RenderToBsonDocument();

            Assert.True(filterDefinitionBson.ElementCount == 1);
            Assert.True(filterDefinitionBson["_id"].AsGuid == deviceEvent.Id);

            Assert.True(updateDefinitionBson.ElementCount == 1);
            Assert.True(updateDefinitionBson["$set"].IsBsonDocument);

            var setBsonDocument = updateDefinitionBson["$set"].AsBsonDocument;
            var fixedBsonDate = setBsonDocument["Date"].ToLocalTime().AddMilliseconds(-setBsonDocument["Date"].ToLocalTime().Millisecond);            

            Assert.True(setBsonDocument.ElementCount == 6);
            Assert.True(setBsonDocument["Deleted"].AsBoolean == deviceEvent.Deleted);
            Assert.True(setBsonDocument["Type"].AsInt32 == (int)deviceEvent.Type);           
            Assert.True(fixedBsonDate.ToString("yyyyMMddhhmmss") == deviceEvent.Date.ToString("yyyyMMddhhmmss"));
            Assert.True(setBsonDocument["TypeName"].AsString == deviceEvent.TypeName);
            Assert.True(setBsonDocument["DeviceId"].AsGuid == deviceEvent.DeviceId);
            Assert.True(setBsonDocument["EventDetails"].IsBsonDocument);

            var detailsBson = setBsonDocument["EventDetails"].AsBsonDocument;
            Assert.Null(detailsBson.RawValue);
        }

        [Fact(DisplayName = "UpdateWithDetails_CallsUpdateOne")]
        [Trait("Category", "DeviceEvents.DataAccess.DeviceEventDao")]
        public async Task UpdateWithDetails_CallsUpdateOne()
        {
            var deviceEvent = DeviceEvent.NewCustomEvent(Guid.NewGuid(), "Lol", new { Data = "Hou hou hou" });
            UpdateDefinition<DeviceEvent> updateDefinition = null;
            FilterDefinition<DeviceEvent> filterDefinition = null;

            var mockedMongoDbCollection = new Mock<IMongoCollection<DeviceEvent>>(MockBehavior.Strict);
            var mongoDatabaseMock = new Mock<IMongoDatabase>(MockBehavior.Strict);
            var updateResultMock = new Mock<UpdateResult>();

            mockedMongoDbCollection.Setup(m => m.UpdateOneAsync(It.IsAny<FilterDefinition<DeviceEvent>>(), It.IsAny<UpdateDefinition<DeviceEvent>>(), It.IsAny<UpdateOptions>(), It.IsAny<CancellationToken>()))
                            .Callback((FilterDefinition<DeviceEvent> filter, UpdateDefinition<DeviceEvent> update, UpdateOptions options, CancellationToken cancellationToken) =>
                            {
                                filterDefinition = filter;
                                updateDefinition = update;
                            })
                            .Returns(Task.FromResult(updateResultMock.Object));

            mongoDatabaseMock.Setup(m => m.GetCollection<DeviceEvent>(CollectionName, null)).Returns(() => { return mockedMongoDbCollection.Object; });

            var dao = new DeviceEventDao(mongoDatabaseMock.Object);
            await dao.UpdateAsync(deviceEvent);

            mockedMongoDbCollection.Verify(m => m.UpdateOneAsync(It.IsAny<FilterDefinition<DeviceEvent>>(), It.IsAny<UpdateDefinition<DeviceEvent>>(), It.IsAny<UpdateOptions>(), It.IsAny<CancellationToken>()), Times.Once());
            mongoDatabaseMock.Verify(m => m.GetCollection<DeviceEvent>(CollectionName, null), Times.Once());

            var filterDefinitionBson = filterDefinition.RenderToBsonDocument();
            var updateDefinitionBson = updateDefinition.RenderToBsonDocument();

            var setBsonDocument = updateDefinitionBson["$set"].AsBsonDocument;
            var fixedBsonDate = setBsonDocument["Date"].ToLocalTime().AddMilliseconds(-setBsonDocument["Date"].ToLocalTime().Millisecond);

            Assert.True(setBsonDocument.ElementCount == 6);
            Assert.True(setBsonDocument["Deleted"].AsBoolean == deviceEvent.Deleted);
            Assert.True(setBsonDocument["Type"].AsInt32 == (int)deviceEvent.Type);
            Assert.True(fixedBsonDate.ToString("yyyyMMddhhmmss") == deviceEvent.Date.ToString("yyyyMMddhhmmss"));
            Assert.True(setBsonDocument["TypeName"].AsString == deviceEvent.TypeName);
            Assert.True(setBsonDocument["DeviceId"].AsGuid == deviceEvent.DeviceId);
            Assert.True(setBsonDocument["EventDetails"].IsBsonDocument);

            var detailsBson = setBsonDocument["EventDetails"].AsBsonDocument;
            Assert.True((JObject.Parse(detailsBson["_v"].ToJson()) as dynamic).Data == deviceEvent.Details.Data);
        }
    }


}
