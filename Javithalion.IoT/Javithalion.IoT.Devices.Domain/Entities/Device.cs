using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.Devices.Domain.Entities
{
    public class Device
    {
        public Guid Id { get; protected set; }

        public string Name { get; protected set; }

        public bool Disabled { get; protected set; }

        protected Device() { }

        public static Device CreateNew(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Provided name for device was null or empty. Device name is mandatory");

            return new Device()
            {
                Name = name,
                Disabled = false
            };
        }

        public Device WithName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Provided name for device was null or empty. Device name is mandatory");

            Name = name;
            return this;
        }

        public Device Disable()
        {
            Disabled = true;
            return this;
        }
    }
}
