using Javithalion.IoT.Devices.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.Devices.DataAccess.Write
{
    public class DevicesContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }

        public DevicesContext(DbContextOptions<DevicesContext> options)
            : base(options)
        { }
    }
}
