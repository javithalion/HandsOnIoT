using Javithalion.IoT.Infraestructure.Bus.BusEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.Infraestructure.ModelBus
{
    public interface IServiceBus
    {
        Task SendAsync<T>(T command) where T : Command;
        Task RaiseEventAsync<T>(T theEvent) where T : Event;
        void RegisterHandler<HandlerType, MessageType>() where MessageType : Message where HandlerType : Message;
    }
}
