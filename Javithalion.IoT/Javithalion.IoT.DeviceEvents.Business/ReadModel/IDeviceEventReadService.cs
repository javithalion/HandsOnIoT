using Javithalion.IoT.DeviceEvents.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.DeviceEvents.Business.ReadModel
{
    interface IDeviceEventReadService
    {
        Task<IList<DeviceEvent>> FindAllAsync();
    }
}
