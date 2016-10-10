using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.Infraestructure.Bus.BusEntities
{
    public interface ICanHandle<T> where T : Message
    {
        void Handle(T message);
    }
}
