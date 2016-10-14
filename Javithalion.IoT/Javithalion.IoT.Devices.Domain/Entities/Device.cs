using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.Devices.Domain.Entities
{
    public class Device
    {       
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        protected Device() { }

        public static Device CreateNew(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Provided name for device was null or empty. Device name is mandatory");

            return new Device() { Name = name };
        }
    }
}
