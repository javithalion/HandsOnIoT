using Javithalion.IoT.DeviceEvents.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.DeviceEvents.Business.ReadModel
{
    public interface IDeviceEventReadService
    {
        Task<IList<DeviceEvent>> FindAllForDeviceAsync(string deviceId);

        Task<DeviceEvent> GetAsync(string deviceId, string eventId);
    }
}
