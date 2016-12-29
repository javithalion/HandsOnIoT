using Javithalion.IoT.DeviceEvents.DataAccess.DAOs;
using Javithalion.IoT.DeviceEvents.Domain.Entities;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Javithalion.IoT.DeviceEvents.DataAccess.Tests.DaoTestFixtures
{
    public class DeviceEventDaoTestFixture
    {
        //REMARK :: Dao internals are not tested since we are not supposed to unit test third party frameworks/drivers. 
        //          Integration tests are supposed to use them

        [Fact(DisplayName = "FindAll_CallsGetCollection")]
        [Trait("Category", "DeviceEvents.DataAccess.DeviceEventDao")]
        public async Task FindAll_CallsGetCollection()
        {
            string collectionName = "DeviceEventCollection";

            var mockedMongoDbCollection = new Mock<IMongoCollection<DeviceEvent>>();
            var mongoDatabaseMock = new Mock<IMongoDatabase>(MockBehavior.Strict);
            mongoDatabaseMock.Setup(m => m.GetCollection<DeviceEvent>(collectionName, null)).Returns(() => { return mockedMongoDbCollection.Object; });

            var dao = new DeviceEventDao(mongoDatabaseMock.Object);

            var result = dao.AllDeviceEvents();

            mongoDatabaseMock.Verify(m => m.GetCollection<DeviceEvent>(collectionName, null), Times.Once());
        }

        [Fact(DisplayName = "Insert_CallsInsertOne")]
        [Trait("Category", "DeviceEvents.DataAccess.DeviceEventDao")]
        public async Task Insert_CallsInsertOne()
        {
            string collectionName = "DeviceEventCollection";
            var deviceEvent = DeviceEvent.NewStartUpEvent(Guid.NewGuid());
            DeviceEvent insertedItem = null;

            var mockedMongoDbCollection = new Mock<IMongoCollection<DeviceEvent>>(MockBehavior.Strict);
            var mongoDatabaseMock = new Mock<IMongoDatabase>(MockBehavior.Strict);

            mockedMongoDbCollection.Setup(m => m.InsertOneAsync(deviceEvent, It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()))
                            .Callback((DeviceEvent item, InsertOneOptions options, CancellationToken cancellationToken) => { insertedItem = item; })
                            .Returns(Task.FromResult(new object()));

            mongoDatabaseMock.Setup(m => m.GetCollection<DeviceEvent>(collectionName, null)).Returns(() => { return mockedMongoDbCollection.Object; });

            var dao = new DeviceEventDao(mongoDatabaseMock.Object);

            await dao.InsertAsync(deviceEvent);

            mockedMongoDbCollection.Verify(m => m.InsertOneAsync(deviceEvent, It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()), Times.Once());
            mongoDatabaseMock.Verify(m => m.GetCollection<DeviceEvent>(collectionName, null), Times.Once());            
            Assert.Equal(deviceEvent, insertedItem);
        }

        [Fact(DisplayName = "Insert_CallsInsertOne")]
        [Trait("Category", "DeviceEvents.DataAccess.DeviceEventDao")]
        public async Task Update_CallsUpdateOne()
        {           
            Assert.True(false, "Not implemented");
        }
    }


}
