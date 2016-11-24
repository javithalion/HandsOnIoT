using Javithalion.IoT.Devices.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.Devices.DataAccess.Write.Extensions
{
    public static class OperativeSystemDataAccessExtensions
    {
        public static async Task<OperativeSystem> WithIdAsync(this DbSet<OperativeSystem> collection, int id)
        {
            return await collection.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
