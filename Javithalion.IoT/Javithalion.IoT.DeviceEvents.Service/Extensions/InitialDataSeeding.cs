using Javithalion.IoT.DeviceEvents.DataAccess.DAOs;
using Javithalion.IoT.DeviceEvents.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Javithalion.IoT.DeviceEvents.Service.Extensions
{
    public static class InitialDataSeeding
    {
        private const string SeedingFileName = "deviceEventsSeedingData.json";

        public static IApplicationBuilder SeedData(this IApplicationBuilder app)
        {
            var seedingfilePath = Path.Combine(Directory.GetCurrentDirectory(), SeedingFileName);
            if (File.Exists(seedingfilePath))
            {
                IList<DeviceEvent> eventsToSeed = new List<DeviceEvent>();
                using (StreamReader file = File.OpenText(seedingfilePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    eventsToSeed = (IList<DeviceEvent>)serializer.Deserialize(file, typeof(IList<DeviceEvent>));
                }

                var deviceEventDao = (IDeviceEventDao)app.ApplicationServices.GetService(typeof(IDeviceEventDao));

                foreach(var deviceEvent in eventsToSeed)
                {
                    deviceEventDao.InsertAsync(deviceEvent);
                }
            }

            return app;
        }
    }
}
