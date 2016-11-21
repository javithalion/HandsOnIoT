using Javithalion.IoT.Devices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.Devices.DataAccess.Write.Extensions
{
    public static class DevicesContextExtensions
    {
        private const int NumberOfSeededDevices = 60;

        public  static void EnsureSeeding(this DevicesContext context)
        {
            if (!context.Devices.Any())
            {
                for (int i = 0; i < NumberOfSeededDevices; i++)
                {
                    var auxDevice = Device.CreateNew($"Device #{i + 1}");
                    context.Devices.Add(auxDevice);
                }
            }

            context.SaveChanges();
        }
    }
}
