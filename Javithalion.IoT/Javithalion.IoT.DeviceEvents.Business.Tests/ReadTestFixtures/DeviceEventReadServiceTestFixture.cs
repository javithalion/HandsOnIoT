using AutoMapper;
using Javithalion.IoT.DeviceEvents.Business.ReadModel;
using Javithalion.IoT.DeviceEvents.Business.Tests.ReadTestFixtures.Support;
using Javithalion.IoT.DeviceEvents.DataAccess.DAOs;
using Javithalion.IoT.DeviceEvents.Domain.Entities;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Javithalion.IoT.DeviceEvents.Business.Tests.ReadTestFixtures
{

    public class DeviceEventReadServiceTestFixture
    {
        [Fact(DisplayName = "GetAllDeviceEvents_NoDeviceEvents")]
        [Trait("Category", "DeviceEvents.Business.Read")]
        public async void GetAllDeviceEvents_NoDeviceEvents()
        {
            //Arrange
            var nonExisitingId = Guid.NewGuid();

            var listOfEvents = new List<DeviceEvent>().AsQueryable().AsMongoQueryable();

            var deviceEventsDaoMock = new Mock<IDeviceEventDao>();
            deviceEventsDaoMock.Setup(m => m.AllDeviceEvents()).Returns(listOfEvents);
            IMapper mapper = MapperFactory.GetReaderServiceMapper();

            var deviceReadService = new DeviceEventReadService(deviceEventsDaoMock.Object, mapper);

            //Act
            var result = await deviceReadService.FindAllForDeviceAsync(nonExisitingId);

            //Assert
            Assert.NotNull(result);
            Assert.False(result.Any());
        }



        [Fact(DisplayName = "GetAllDeviceEvents_Ok")]
        [Trait("Category", "DeviceEvents.Business.Read")]
        public async void GetAllDeviceEvents_Ok()
        {
            //Arrange
            var numberOfDeviceEvents = 26;

            var deviceId = Guid.NewGuid();
            var otherDeviceId = Guid.NewGuid();

            var deviceEvents = DeviceEventsFactory.GetListOfDeviceEvents(deviceId, numberOfDeviceEvents);
            var otherDeviceEvents = DeviceEventsFactory.GetListOfDeviceEvents(otherDeviceId, numberOfDeviceEvents);
            var listOfEvents = deviceEvents.Union(otherDeviceEvents).AsMongoQueryable();

            var deviceEventsDaoMock = new Mock<IDeviceEventDao>();
            deviceEventsDaoMock.Setup(m => m.AllDeviceEvents()).Returns(listOfEvents);
            IMapper mapper = MapperFactory.GetReaderServiceMapper();

            var deviceReadService = new DeviceEventReadService(deviceEventsDaoMock.Object, mapper);

            //Act
            var result = await deviceReadService.FindAllForDeviceAsync(deviceId);

            //Assert
            Assert.True(result.Count() == numberOfDeviceEvents);
            Assert.True(result.All(de => de.DeviceId == deviceId));
            Assert.True(deviceEvents.ToList().All(de => result.Any(r =>
                                                                        de.Id == r.Id &&
                                                                        de.Date.ToString("yyyyMMddhhmmss") == r.Date.ToLocalTime().ToString("yyyyMMddhhmmss") &&
                                                                        JsonConvert.SerializeObject(de.Details) == JsonConvert.SerializeObject(r.Details) &&
                                                                        de.DeviceId == r.DeviceId &&
                                                                        de.TypeName == r.Type)));
        }

        [Fact(DisplayName = "GetDeviceEvent_Ok")]
        [Trait("Category", "DeviceEvents.Business.Read")]
        public async void GetDeviceEvent_Ok()
        {
            //Arrange
            var numberOfDeviceEvents = 26;

            var deviceId = Guid.NewGuid();
            var otherDeviceId = Guid.NewGuid();

            var deviceEvents = DeviceEventsFactory.GetListOfDeviceEvents(deviceId, numberOfDeviceEvents);
            var otherDeviceEvents = DeviceEventsFactory.GetListOfDeviceEvents(otherDeviceId, numberOfDeviceEvents);
            var randomEvent = deviceEvents.Union(otherDeviceEvents).LastOrDefault();

            var listOfEvents = deviceEvents.Union(otherDeviceEvents).AsMongoQueryable();

            var deviceEventsDaoMock = new Mock<IDeviceEventDao>();
            deviceEventsDaoMock.Setup(m => m.AllDeviceEvents()).Returns(listOfEvents);
            IMapper mapper = MapperFactory.GetReaderServiceMapper();

            var deviceReadService = new DeviceEventReadService(deviceEventsDaoMock.Object, mapper);

            //Act
            var result = await deviceReadService.GetAsync(randomEvent.Id);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Id == randomEvent.Id);
            Assert.True(result.Date.ToLocalTime().ToString("yyyyMMddhhmmss") == randomEvent.Date.ToString("yyyyMMddhhmmss"));
            Assert.True(JsonConvert.SerializeObject(result.Details) == JsonConvert.SerializeObject(randomEvent.Details));
            Assert.True(result.DeviceId == randomEvent.DeviceId);
            Assert.True(result.Type == randomEvent.TypeName);
        }

        [Fact(DisplayName = "GetDeviceEvent_NonExistingId")]
        [Trait("Category", "DeviceEvents.Business.Read")]
        public async void GetDeviceEvent_NonExistingId()
        {
            //Arrange
            var numberOfDeviceEvents = 26;

            var deviceId = Guid.NewGuid();
            var otherDeviceId = Guid.NewGuid();
            var nonExisitingId = Guid.NewGuid();

            var deviceEvents = DeviceEventsFactory.GetListOfDeviceEvents(deviceId, numberOfDeviceEvents);
            var otherDeviceEvents = DeviceEventsFactory.GetListOfDeviceEvents(otherDeviceId, numberOfDeviceEvents);

            var listOfEvents = deviceEvents.Union(otherDeviceEvents).AsMongoQueryable();

            var deviceEventsDaoMock = new Mock<IDeviceEventDao>();
            deviceEventsDaoMock.Setup(m => m.AllDeviceEvents()).Returns(listOfEvents);
            IMapper mapper = MapperFactory.GetReaderServiceMapper();

            var deviceReadService = new DeviceEventReadService(deviceEventsDaoMock.Object, mapper);

            //Act
            var result = await deviceReadService.GetAsync(nonExisitingId);

            //Assert
            Assert.Null(result);
        }

        [Fact(DisplayName = "GetDeviceEvent_NoDeviceEvents")]
        [Trait("Category", "DeviceEvents.Business.Read")]
        public async void GetDeviceEvent_NoDeviceEvents()
        {
            //Arrange
            var nonExisitingId = Guid.NewGuid();

            var listOfEvents = new List<DeviceEvent>().AsQueryable().AsMongoQueryable();

            var deviceEventsDaoMock = new Mock<IDeviceEventDao>();
            deviceEventsDaoMock.Setup(m => m.AllDeviceEvents()).Returns(listOfEvents);
            IMapper mapper = MapperFactory.GetReaderServiceMapper();

            var deviceReadService = new DeviceEventReadService(deviceEventsDaoMock.Object, mapper);

            //Act
            var result = await deviceReadService.GetAsync(nonExisitingId);

            //Assert
            Assert.Null(result);
        }
    }
}
