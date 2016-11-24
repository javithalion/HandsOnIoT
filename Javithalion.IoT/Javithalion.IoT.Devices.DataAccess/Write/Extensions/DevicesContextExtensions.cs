using Javithalion.IoT.Devices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.Devices.DataAccess.Write.Extensions
{
    public static class DevicesContextExtensions
    {
        private const int NumberOfSeededDevices = 10;

        public static void EnsureSeeding(this DevicesContext context)
        {
            if (!context.OperativeSystems.Any())
            {
                var defaultOperativeSystems = GetDefaultOperativeSystems();
                foreach (var os in defaultOperativeSystems)
                {
                    context.OperativeSystems.Add(os);
                }
            }

            if (!context.Devices.Any())
            {
                var defaultOperativeSystem = GetDefaultOperativeSystems().First();
                for (int i = 0; i < NumberOfSeededDevices; i++)
                {
                    var auxDevice = Device.CreateNew($"Device #{i + 1}", defaultOperativeSystem);
                    context.Devices.Add(auxDevice);
                }
            }

            context.SaveChanges();
        }

        private static IEnumerable<OperativeSystem> GetDefaultOperativeSystems()
        {
            return new List<OperativeSystem>()
            {
                OperativeSystem.Undefined,
                new OperativeSystem(1, "Windows"),
                new OperativeSystem(2, "Windows IoT"),
                new OperativeSystem(3, "Android"),
                new OperativeSystem(4, "iOS"),
                new OperativeSystem(5, "Linux")
            };
        }
    }
}