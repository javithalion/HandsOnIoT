using Javithalion.IoT.DeviceEvents.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Javithalion.IoT.DeviceEvents.DataAccess.DAOs
{
    public interface IDeviceEventDao
    {
        Task<IList<DeviceEvent>> FindAll();
    }
}