using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Javithalion.IoT.Devices.Domain.Entities
{
    public class Device
    {
        public Guid Id { get; protected set; }

        public string Name { get; protected set; }

        public bool Disabled { get; protected set; }

        public OperativeSystem OperativeSystem { get; private set; }

        public IPAddress IpAddress { get; private set; }       

        protected Device() { }

        public static Device CreateNew(string name, OperativeSystem operativeSystem)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Provided name for device was null or empty. Device name is mandatory");

            return new Device()
            {
                Name = name,
                Disabled = false,
                OperativeSystem = operativeSystem,
                IpAddress = UndefinedIPAddress.Value
            };
        }

        public Device WithName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Provided name for device was null or empty. Device name is mandatory");

            Name = name;
            return this;
        }

        public Device Running(OperativeSystem operativeSystem)
        {
            OperativeSystem = operativeSystem;
            return this;
        }

        public Device WithIpAddress(string ipAddress)
        {
            IpAddress = ParseStringToIpAddress(ipAddress);

            return this;
        }

        public IPAddress ParseStringToIpAddress(string ipString)
        {
            if (string.IsNullOrWhiteSpace(ipString))
                throw new ArgumentException("Provided IP address was null or empty");

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
                throw new ArgumentException($"Provided IP address {ipString} didn't has the expected format ###.###.###.###");

            IPAddress address;
            if (!IPAddress.TryParse(ipString, out address))
                throw new ArgumentException($"Provided string {ipString} is not a valid value for an IP address");

            return address;
        }

        public Device Disable()
        {
            Disabled = true;
            return this;
        }


        private class UndefinedIPAddress
        {
            private UndefinedIPAddress()
            {
            }

            public static IPAddress Value
            {
                get { return new IPAddress(0x0); }
            }
        }
    }
}
