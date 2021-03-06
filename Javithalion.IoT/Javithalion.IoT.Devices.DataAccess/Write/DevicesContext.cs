﻿using Javithalion.IoT.Devices.Domain.Entities;
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
        public DbSet<OperativeSystem> OperativeSystems { get; set; }

        public DevicesContext(DbContextOptions<DevicesContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OperativeSystem>().ToTable("OperativeSystems");
            modelBuilder.Entity<OperativeSystem>().HasKey(os => os.Id);            

            modelBuilder.Entity<Device>().ToTable("Devices");
            modelBuilder.Entity<Device>().HasKey(d => d.Id);
            modelBuilder.Entity<Device>().Property(x => x.Name).HasMaxLength(30);            
            modelBuilder.Entity<Device>().HasOne(x => x.OperativeSystem);
        }
    }
}
