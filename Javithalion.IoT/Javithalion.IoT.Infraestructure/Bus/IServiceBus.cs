using Javithalion.IoT.Infraestructure.Bus.BusEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.Infraestructure.ModelBus
{
    public interface IServiceBus
    {
        Task Send<T>(T command) where T : Command;

        Task Publish<T>(T @event) where T : Event;

        void RegisterHandler<T>(ICanHandle<T> handler) where T : Message;
    }
}
