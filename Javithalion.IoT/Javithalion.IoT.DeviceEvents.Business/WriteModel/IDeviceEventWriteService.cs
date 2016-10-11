using Javithalion.IoT.DeviceEvents.Business.ReadModel.DTOs;
using Javithalion.IoT.DeviceEvents.Business.WriteModel.Commands;
using Javithalion.IoT.DeviceEvents.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Javithalion.IoT.DeviceEvents.Business.WriteModel
{
    public interface IDeviceEventWriteService
    {
        Task<DeviceEventDto> AddDeviceEventAsync(CreateDeviceEventCommand createCommand);

        Task RemoveDeviceEventFromDeviceAsync(string deviceId, string eventId);
    }
}