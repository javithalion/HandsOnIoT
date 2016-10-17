using Javithalion.IoT.Devices.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.Devices.DataAccess.Write.Extensions
{
    public static class DeviceDataAccessExtensions
    {
        public static async Task<Device> WithIdAsync(this DbSet<Device> collection, Guid id)
        {
            return await collection.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
