using Javithalion.IoT.DeviceEvents.Business.ReadModel.DTOs;
using Javithalion.IoT.DeviceEvents.Business.WriteModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.DeviceEvents.Business
{
    public interface IDeviceEventWriteService
    {
        Task<DeviceEventDto> CreateAsync(CreateDeviceEventCommand createCommand);

        Task<DeviceEventDto> UpdateAsync(UpdateDeviceEventCommand updateCommand);

        Task<DeviceEventDto> DeleteAsync(DeleteDeviceEventCommand deleteCommand);
    }
}
