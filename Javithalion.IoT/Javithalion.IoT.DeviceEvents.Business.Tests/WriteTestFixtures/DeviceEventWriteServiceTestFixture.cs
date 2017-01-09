using AutoMapper;
using Javithalion.IoT.DeviceEvents.Business.Tests.ReadTestFixtures.Support;
using Javithalion.IoT.DeviceEvents.Business.WriteModel;
using Javithalion.IoT.DeviceEvents.Business.WriteModel.Commands;
using Javithalion.IoT.DeviceEvents.DataAccess.DAOs;
using Javithalion.IoT.DeviceEvents.Domain.Entities;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Javithalion.IoT.DeviceEvents.Business.Tests.WriteTestFixtures
{
    public class DeviceEventWriteServiceTestFixture
    {

        [Fact(DisplayName = "InsertOne_Ok")]
        [Trait("Category", "DeviceEvents.Business.Write")]
        public async void InsertOne_Ok()
        {
            //Arrange
            DeviceEvent newDeviceEvent = null;
            var deviceId = Guid.NewGuid();
            var createCommand = new CreateDeviceEventCommand()
            {
                DeviceId = deviceId,
                EventType = WriteModel.Commands.EventType.StartUp,
                Details = new { Data = "my start up!" },
                TypeName = null
            };

            var deviceEventsDaoMock = new Mock<IDeviceEventDao>(MockBehavior.Strict);
            deviceEventsDaoMock.Setup(m => m.InsertAsync(It.IsAny<DeviceEvent>()))
                               .Returns(Task.FromResult(newDeviceEvent))
                               .Callback((DeviceEvent theEvent) =>
                               {
                                   newDeviceEvent = theEvent;
                               });

            IMapper mapper = MapperFactory.GetReaderServiceMapper();

            var deviceEventWriteService = new DeviceEventWriteService(deviceEventsDaoMock.Object, mapper);

            //Act
            var result = await deviceEventWriteService.CreateAsync(createCommand);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(newDeviceEvent);
           
            Assert.True(newDeviceEvent.Type == Domain.Entities.EventType.StartUp);
            Assert.True(newDeviceEvent.DeviceId == createCommand.DeviceId);
            Assert.True(newDeviceEvent.Date.ToString("yyyyMMddhhmmss") == DateTime.Now.ToString("yyyyMMddhhmmss"));
            Assert.True(newDeviceEvent.TypeName == Domain.Entities.EventType.StartUp.ToString());
            Assert.True(JsonConvert.SerializeObject(newDeviceEvent.Details) == JsonConvert.SerializeObject(string.Empty));

            Assert.True(result.Id == newDeviceEvent.Id);
            Assert.True(result.DeviceId == newDeviceEvent.DeviceId);
            Assert.True(result.Date.ToString("yyyyMMddhhmmss") == newDeviceEvent.Date.ToString("yyyyMMddhhmmss"));
            Assert.True(result.Type == newDeviceEvent.TypeName);
            Assert.True(JsonConvert.SerializeObject(result.Details) == JsonConvert.SerializeObject(string.Empty));
        }

        [Fact(DisplayName = "InsertOne_WrongData")]
        [Trait("Category", "DeviceEvents.Business.Write")]
        public async void InsertOne_WrongData()
        {
            //Arrange            
            var createCommand = new CreateDeviceEventCommand()
            {
                DeviceId = Guid.Empty,
                EventType = WriteModel.Commands.EventType.StartUp,
                Details = new { Data = "my start up!" },
                TypeName = null
            };

            var deviceEventsDaoMock = new Mock<IDeviceEventDao>(MockBehavior.Strict);
            IMapper mapper = MapperFactory.GetReaderServiceMapper();

            var deviceEventWriteService = new DeviceEventWriteService(deviceEventsDaoMock.Object, mapper);

            //Act
            var result = await Assert.ThrowsAsync<ArgumentException>(() => deviceEventWriteService.CreateAsync(createCommand));

            //Assert     
            Assert.Contains("Device event should be related with a valid device", result.Message);
        }

        [Fact(DisplayName = "UpdateOne_Ok")]
        [Trait("Category", "DeviceEvents.Business.Write")]
        public async void UpdateOne_Ok()
        {
            //Arrange    
            DeviceEvent updatedEvent = null;         
            var numberOfDeviceEvents = 30;
            var deviceId = Guid.NewGuid();
            var otherDeviceId = Guid.NewGuid();

            var deviceEvents = DeviceEventsFactory.GetListOfDeviceEvents(deviceId, numberOfDeviceEvents);
            var otherDeviceEvents = DeviceEventsFactory.GetListOfDeviceEvents(otherDeviceId, numberOfDeviceEvents);
            var randomEvent = deviceEvents.Union(otherDeviceEvents).LastOrDefault();

            var listOfEvents = deviceEvents.Union(otherDeviceEvents).AsMongoQueryable();

            var deviceEventsDaoMock = new Mock<IDeviceEventDao>(MockBehavior.Strict);
            deviceEventsDaoMock.Setup(m => m.AllDeviceEvents()).Returns(listOfEvents);
            deviceEventsDaoMock.Setup(m => m.UpdateAsync(It.IsAny<DeviceEvent>()))
                              .Returns(Task.FromResult(updatedEvent))
                              .Callback((DeviceEvent theEvent) =>
                              {
                                  updatedEvent = theEvent;
                              });

            var updateCommand = new UpdateDeviceEventCommand()
            {
                EventId = randomEvent.Id,
                Details = new { Data = "New details!" },
            };

            IMapper mapper = MapperFactory.GetReaderServiceMapper();
            var deviceEventWriteService = new DeviceEventWriteService(deviceEventsDaoMock.Object, mapper);

            //Act
            var result = await deviceEventWriteService.UpdateAsync(updateCommand);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(updatedEvent);            

            Assert.True(updatedEvent.Id == updateCommand.EventId);
            Assert.True(updatedEvent.Type == randomEvent.Type);
            Assert.True(updatedEvent.DeviceId == randomEvent.DeviceId);
            Assert.True(updatedEvent.Date.ToLocalTime().ToString("yyyyMMddhhmmss") == randomEvent.Date.ToString("yyyyMMddhhmmss"));
            Assert.True(updatedEvent.TypeName == randomEvent.TypeName);
            Assert.True(updatedEvent.Deleted == randomEvent.Deleted);
            Assert.True(JsonConvert.SerializeObject(updatedEvent.Details) == JsonConvert.SerializeObject(updateCommand.Details));

            Assert.True(result.Id == updatedEvent.Id);
            Assert.True(result.DeviceId == updatedEvent.DeviceId);
            Assert.True(result.Date.ToString("yyyyMMddhhmmss") == updatedEvent.Date.ToString("yyyyMMddhhmmss"));
            Assert.True(result.Type == updatedEvent.TypeName);
            Assert.True(JsonConvert.SerializeObject(result.Details) == JsonConvert.SerializeObject(updatedEvent.Details));
        }

        [Fact(DisplayName = "UpdateOne_WrongData")]
        [Trait("Category", "DeviceEvents.Business.Write")]
        public async void UpdateOne_WrongData()
        {
            //Arrange            
            var updateCommand = new UpdateDeviceEventCommand()
            {
                EventId = Guid.NewGuid(),
                Details = new { Data = "my event!" }
            };

            var numberOfDeviceEvents = 30;
            var deviceId = Guid.NewGuid();
            var otherDeviceId = Guid.NewGuid();

            var deviceEvents = DeviceEventsFactory.GetListOfDeviceEvents(deviceId, numberOfDeviceEvents);
            var otherDeviceEvents = DeviceEventsFactory.GetListOfDeviceEvents(otherDeviceId, numberOfDeviceEvents);

            var listOfEvents = deviceEvents.Union(otherDeviceEvents).AsMongoQueryable();

            var deviceEventsDaoMock = new Mock<IDeviceEventDao>(MockBehavior.Strict);
            deviceEventsDaoMock.Setup(m => m.AllDeviceEvents()).Returns(listOfEvents);

            IMapper mapper = MapperFactory.GetReaderServiceMapper();

            var deviceEventWriteService = new DeviceEventWriteService(deviceEventsDaoMock.Object, mapper);

            //Act & Assert
            var result = await Assert.ThrowsAsync<KeyNotFoundException>(() => deviceEventWriteService.UpdateAsync(updateCommand));
        }

        [Fact(DisplayName = "DeleteOne_Ok")]
        [Trait("Category", "DeviceEvents.Business.Write")]
        public async void DeleteOne_Ok()
        {
            //Arrange    
            DeviceEvent deletedEvent = null;
            var numberOfDeviceEvents = 30;
            var deviceId = Guid.NewGuid();
            var otherDeviceId = Guid.NewGuid();

            var deviceEvents = DeviceEventsFactory.GetListOfDeviceEvents(deviceId, numberOfDeviceEvents);
            var otherDeviceEvents = DeviceEventsFactory.GetListOfDeviceEvents(otherDeviceId, numberOfDeviceEvents);
            var randomEvent = deviceEvents.Union(otherDeviceEvents).LastOrDefault();

            var listOfEvents = deviceEvents.Union(otherDeviceEvents).AsMongoQueryable();

            var deviceEventsDaoMock = new Mock<IDeviceEventDao>(MockBehavior.Strict);
            deviceEventsDaoMock.Setup(m => m.AllDeviceEvents()).Returns(listOfEvents);
            deviceEventsDaoMock.Setup(m => m.UpdateAsync(It.IsAny<DeviceEvent>()))
                              .Returns(Task.FromResult(deletedEvent))
                              .Callback((DeviceEvent theEvent) =>
                              {
                                  deletedEvent = theEvent;
                              });

            var deleteCommand = new DeleteDeviceEventCommand()
            {
                EventId = randomEvent.Id               
            };

            IMapper mapper = MapperFactory.GetReaderServiceMapper();
            var deviceEventWriteService = new DeviceEventWriteService(deviceEventsDaoMock.Object, mapper);

            //Act
            var result = await deviceEventWriteService.DeleteAsync(deleteCommand);

            //Assert           
            Assert.NotNull(result);
            Assert.NotNull(deletedEvent);

            Assert.True(deletedEvent.Id == deleteCommand.EventId);
            Assert.True(deletedEvent.Type == randomEvent.Type);
            Assert.True(deletedEvent.DeviceId == randomEvent.DeviceId);
            Assert.True(deletedEvent.Date.ToLocalTime().ToString("yyyyMMddhhmmss") == randomEvent.Date.ToString("yyyyMMddhhmmss"));
            Assert.True(deletedEvent.TypeName == randomEvent.TypeName);
            Assert.True(deletedEvent.Deleted);
            Assert.True(JsonConvert.SerializeObject(deletedEvent.Details) == JsonConvert.SerializeObject(randomEvent.Details));

            Assert.True(result.Id == deletedEvent.Id);
            Assert.True(result.DeviceId == deletedEvent.DeviceId);
            Assert.True(result.Date.ToString("yyyyMMddhhmmss") == deletedEvent.Date.ToString("yyyyMMddhhmmss"));
            Assert.True(result.Type == deletedEvent.TypeName);
            Assert.True(JsonConvert.SerializeObject(result.Details) == JsonConvert.SerializeObject(deletedEvent.Details));

        }

        [Fact(DisplayName = "DeleteOne_WrongData")]
        [Trait("Category", "DeviceEvents.Business.Write")]
        public async void DeleteOne_WrongData()
        {
            //Arrange            
            var deleteCommand = new DeleteDeviceEventCommand()
            {
                EventId = Guid.NewGuid()              
            };

            var numberOfDeviceEvents = 30;
            var deviceId = Guid.NewGuid();
            var otherDeviceId = Guid.NewGuid();

            var deviceEvents = DeviceEventsFactory.GetListOfDeviceEvents(deviceId, numberOfDeviceEvents);
            var otherDeviceEvents = DeviceEventsFactory.GetListOfDeviceEvents(otherDeviceId, numberOfDeviceEvents);

            var listOfEvents = deviceEvents.Union(otherDeviceEvents).AsMongoQueryable();

            var deviceEventsDaoMock = new Mock<IDeviceEventDao>(MockBehavior.Strict);
            deviceEventsDaoMock.Setup(m => m.AllDeviceEvents()).Returns(listOfEvents);

            IMapper mapper = MapperFactory.GetReaderServiceMapper();

            var deviceEventWriteService = new DeviceEventWriteService(deviceEventsDaoMock.Object, mapper);

            //Act & Assert
            var result = await Assert.ThrowsAsync<KeyNotFoundException>(() => deviceEventWriteService.DeleteAsync(deleteCommand));
        }
    }
}
