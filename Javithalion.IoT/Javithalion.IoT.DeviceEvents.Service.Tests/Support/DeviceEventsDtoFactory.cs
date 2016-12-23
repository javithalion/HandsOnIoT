using Javithalion.IoT.DeviceEvents.Business.ReadModel.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.DeviceEvents.Service.Tests.Support
{
    public class DeviceEventsDtoFactory
    {
        public static IEnumerable<DeviceEventDto> GetListOfDeviceEvents(Guid deviceId, int number = 10)
        {
            var result = new List<DeviceEventDto>();
            for (int i = 0; i < number; i++)
            {
                result.Add(GetRandomEvent(deviceId));
            }
            return result;
        }

        private static DeviceEventDto GetRandomEvent(Guid deviceId)
        {
            return new DeviceEventDto()
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                Details = DateTime.Now.Ticks.ToString(),
                DeviceId = deviceId,
                Type = "Custom"
            };            
        }
    }
}
