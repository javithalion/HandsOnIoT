using Javithalion.IoT.Infraestructure.Bus.BusEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Javithalion.IoT.Infraestructure.ModelBus
{
    public class ServiceBus : IServiceBus
    {
        private static readonly Dictionary<Type, List<ICanHandle<Message>>> _handlers = new Dictionary<Type, List<ICanHandle<Message>>>();

        public void RegisterHandler<T>(ICanHandle<T> handler) where T : Message
        {
            List<ICanHandle<Message>> handlers;

            if (!_handlers.TryGetValue(typeof(T), out handlers))
            {
                handlers = new List<ICanHandle<Message>>();
                _handlers.Add(typeof(T), handlers);
            }
            handlers.Add((ICanHandle<Message>)handler);
        }

        public async Task Send<T>(T command) where T : Command
        {
            List<ICanHandle<Message>> handlers;

            if (_handlers.TryGetValue(typeof(T), out handlers))
            {
                if (handlers.Count != 1) throw new InvalidOperationException("cannot send to more than one handler");
                await handlers.FirstOrDefault().Handle(command);
            }
            else
                throw new InvalidOperationException($"No handler registered for {command.GetType()}");
        }

        public async Task Publish<T>(T @event) where T : Event
        {
            List<ICanHandle<Message>> handlers;

            if (!_handlers.TryGetValue(@event.GetType(), out handlers)) return;

            foreach (var handler in handlers)
                await Task.Run(() => handler.Handle(@event));
        }
    }
}
