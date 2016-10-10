using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Javithalion.IoT.DeviceEvents.Domain.Entities;
using Javithalion.IoT.DeviceEvents.DataAccess.DAOs;

namespace Javithalion.IoT.DeviceEvents.Business.ReadModel
{
    public class DeviceEventReadService : IDeviceEventReadService
    {
        private readonly IDeviceEventDao _deviceEventServiceDao;

        public DeviceEventReadService(IDeviceEventDao deviceEventDao)
        {
            _deviceEventServiceDao = deviceEventDao;
        }        

        public async Task<IList<DeviceEvent>> FindAllForDeviceAsync(string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
                throw new ArgumentException("Null or empty device id was provided", nameof(deviceId));

            Guid parsedDeviceGuid;
            if(!Guid.TryParse(deviceId,out parsedDeviceGuid))
                throw new ArgumentException("Provided device id has incorrect format", nameof(deviceId));

            return await _deviceEventServiceDao.FindAllForDeviceAsync(parsedDeviceGuid);
        }

        public async Task<DeviceEvent> GetAsync(string deviceId, string eventId)
        {
            if (string.IsNullOrEmpty(deviceId))
                throw new ArgumentException("Null or empty device id was provided", nameof(deviceId));

            Guid parsedDeviceGuid;
            if (!Guid.TryParse(deviceId, out parsedDeviceGuid))
                throw new ArgumentException("Provided device id has incorrect format", nameof(deviceId));

            if (string.IsNullOrEmpty(eventId))
                throw new ArgumentException("Null or empty event id was provided", nameof(eventId));

            Guid parsedEventGuid;
            if (!Guid.TryParse(eventId, out parsedEventGuid))
                throw new ArgumentException("Provided event id has incorrect format", nameof(eventId));


            return await _deviceEventServiceDao.GetAsync(parsedDeviceGuid, parsedEventGuid);
        }
    }
}
