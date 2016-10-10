using Javithalion.IoT.DeviceEvents.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Javithalion.IoT.DeviceEvents.Business.WriteModel
{
    public interface IDeviceEventWriteService
    {
        Task AddDeviceEventToDeviceAsync(string deviceId, DeviceEvent theEvent);

        Task RemoveDeviceEventFromDeviceAsync(string deviceId, string eventId);
    }
}