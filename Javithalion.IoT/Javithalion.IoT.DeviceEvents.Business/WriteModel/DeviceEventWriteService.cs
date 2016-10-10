using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Javithalion.IoT.DeviceEvents.Domain.Entities;

namespace Javithalion.IoT.DeviceEvents.Business.WriteModel
{
    public class DeviceEventWriteService : IDeviceEventWriteService
    {
        public async Task AddDeviceEventToDeviceAsync(string deviceId, DeviceEvent theEvent)
        {
            if (string.IsNullOrEmpty(deviceId))
                throw new ArgumentException("Null or empty device id was provided", nameof(deviceId));

            Guid parsedDeviceGuid;
            if (!Guid.TryParse(deviceId, out parsedDeviceGuid))
                throw new ArgumentException("Provided device id has incorrect format", nameof(deviceId));

            if (theEvent.DeviceId != parsedDeviceGuid)
                throw new ArgumentException("Provided device id is different than the device id the event has", nameof(theEvent));



        }

        public async Task RemoveDeviceEventFromDeviceAsync(string deviceId, string eventId)
        {
            throw new NotImplementedException();
        }
    }
}
