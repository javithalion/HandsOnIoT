using Javithalion.IoT.DeviceEvents.Business.ReadModel.DTOs;
using Javithalion.IoT.DeviceEvents.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.DeviceEvents.Business.ReadModel
{
    public interface IDeviceEventReadService
    {
        Task<IEnumerable<DeviceEventDto>> FindAllForDeviceAsync(string deviceId, int page, int pageSize);

        Task<DeviceEventDto> GetAsync(string eventId);      
    }
}
