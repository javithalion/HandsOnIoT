using Javithalion.IoT.Devices.Business.ReadModel.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.Devices.Business.ReadModel
{
    public interface IDeviceReadService
    {
        Task<IEnumerable<DeviceDto>> FindAllAsync(string searchText = "");

        Task<DeviceDto> GetAsync(Guid id);
    }
}
