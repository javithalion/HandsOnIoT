using Javithalion.IoT.Devices.Business.ReadModel.DTOs;
using Javithalion.IoT.Devices.Business.WriteModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.Devices.Business
{
    public interface IDeviceWriteService
    {
        Task<DeviceDto> CreateAsync(CreateDeviceCommand createCommand);

        Task<DeviceDto> UpdateAsync(UpdateDeviceCommand updateCommand);

        Task<DeviceDto> DeleteAsync(DeleteDeviceCommand deleteCommand);
    }
}
