using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.Infraestructure.Bus.BusEntities
{
    public class Message
    {
        public DateTime TimeStamp { get; protected set; }
        public string SagaId { get; protected set; }
    }
}
