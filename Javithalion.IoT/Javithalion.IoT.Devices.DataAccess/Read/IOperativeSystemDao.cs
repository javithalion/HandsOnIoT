using Javithalion.IoT.Devices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.Devices.DataAccess.Read
{
    public interface IOperativeSystemDao
    {
        Task<IEnumerable<OperativeSystem>> FindAllAsync();
    }
}
