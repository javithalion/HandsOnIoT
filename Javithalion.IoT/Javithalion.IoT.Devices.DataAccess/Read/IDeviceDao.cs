using Javithalion.IoT.Devices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.Devices.DataAccess.Read
{
    public interface IDeviceDao
    {
        Task<IEnumerable<Device>> FindAllAsync(string searchText);

        Task<Device> GetAsync(Guid id);
    }
}
