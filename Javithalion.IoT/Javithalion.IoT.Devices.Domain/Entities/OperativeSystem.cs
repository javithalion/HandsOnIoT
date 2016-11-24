using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.Devices.Domain.Entities
{
    public class OperativeSystem
    {
        private const int UndefinedOperativeSystemId = 0;
        private const string UndefinedOperativeSystemName = "Undefined";

        public int Id { get; private set; }

        public string Name { get; private set; }

        private OperativeSystem()
        { }

        public OperativeSystem(int id, string name)
        {
            if (id <= 0)
                throw new ArgumentException("Id should be greater than zero", nameof(id));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));

            Id = id;
            Name = name;
        }

        public static OperativeSystem Undefined
        {
            get
            {
                return new OperativeSystem() { Id = UndefinedOperativeSystemId, Name = UndefinedOperativeSystemName };
            }
        }
    }
}
