using Javithalion.IoT.DeviceEvents.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.DeviceEvents.Business.Tests.ReadTestFixtures.Support
{
    public static class DeviceEventsFactory
    {
        private static Random _random = new Random();

        public static IQueryable<DeviceEvent> GetListOfDeviceEvents(Guid deviceId, int number = 10)
        {
            var result = new List<DeviceEvent>();
            for (int i = 0; i < number; i++)
            {
                result.Add(GetRandomEvent(deviceId));
            }
            return result.AsQueryable();
        }

        private static DeviceEvent GetRandomEvent(Guid deviceId)
        {
            var randomNumber = _random.Next(0, 4);
            switch (randomNumber % 5)
            {
                case 0:
                    return DeviceEvent.NewStartUpEvent(deviceId);
                case 1:
                    return DeviceEvent.NewTearDownEvent(deviceId);
                case 2:
                    return DeviceEvent.NewResourcesOverviewEvent(deviceId, new { details = "Lol" });
                case 3:
                    return DeviceEvent.NewResourcesDetailedEvent(deviceId, new { details = "Lol" });
                default:
                    return DeviceEvent.NewCustomEvent(deviceId, "RandomEvent");
            }
        }
    }    
}
