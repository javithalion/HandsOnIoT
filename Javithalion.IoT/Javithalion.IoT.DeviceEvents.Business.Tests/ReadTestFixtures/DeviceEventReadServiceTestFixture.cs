using AutoMapper;
using Javithalion.IoT.DeviceEvents.Business.ReadModel;
using Javithalion.IoT.DeviceEvents.Business.Tests.ReadTestFixtures.Support;
using Javithalion.IoT.DeviceEvents.DataAccess.DAOs;
using Moq;
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
        [Fact(DisplayName = "GetAllDeviceEvents_Ok")]
        [Trait("Category", "DeviceEvents.Business.Read")]
        public async void GetAllDeviceEvents_Ok()
        {
            Assert.True(false, "Not implemented");
            
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
            var result = await deviceReadService.FindAllForDeviceAsync(deviceId);

            Assert.True(result.Count() == numberOfDeviceEvents);
            Assert.True(result.All(de => de.DeviceId == deviceId));
            Assert.True(deviceEvents.ToList().All(de => result.FirstOrDefault(r => de.Id == r.Id &&
                                                                                   de.Date == r.Date &&
                                                                                   de.Details == r.Details &&
                                                                                   de.DeviceId == r.DeviceId &&
                                                                                   de.TypeName == r.Type) != null));
        }

        [Fact(DisplayName = "GetDeviceEvent_Ok")]
        [Trait("Category", "DeviceEvents.Business.Read")]        
        public void GetDeviceEvent_Ok()
        {

        }
    }
}
