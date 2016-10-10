using Javithalion.IoT.DeviceEvents.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Javithalion.IoT.DeviceEvents.DataAccess.DAOs
{
    public interface IDeviceEventDao
    {
        Task<IList<DeviceEvent>> FindAllForDeviceAsync(Guid deviceId);
        Task<DeviceEvent> GetAsync(Guid parsedDeviceGuid, Guid parsedEventGuid);
    }
}