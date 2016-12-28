using Javithalion.IoT.DeviceEvents.DataAccess.DAOs;
using Javithalion.IoT.DeviceEvents.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using System;

namespace Javithalion.IoT.DeviceEvents.Service.Extensions
{
    public static class InitialDataSeeding
    {
        public static IApplicationBuilder SeedData(this IApplicationBuilder app)
        {
            //Put additional seeding here
            return app;
        }
    }
}
