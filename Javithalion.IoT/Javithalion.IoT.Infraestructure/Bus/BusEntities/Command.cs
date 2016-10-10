using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.Infraestructure.Bus.BusEntities
{
    public class Command : Message
    {
        public string Name { get; protected set; }
    }
}
