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
        private static IDictionary<Type, Type> RegisteredHandlers = new Dictionary<Type, Type>();
        private readonly IServiceProvider _container;

        public ServiceBus (IServiceProvider container)
        {
            _container = container;
        }

        public void RegisterHandler<HandlerType, MessageType>() where MessageType : Message where HandlerType : Message
        {
            RegisteredHandlers.Add(typeof(HandlerType), typeof(MessageType));
        }

        public async Task SendAsync<T>(T message) where T : Command
        {
            var handlers = RegisteredHandlers.Where(handler => handler.Key == message.GetType());
            foreach(var handler in handlers)
            {               
                var instance = (ICanHandle<T>)_container.GetService(handler.Key);
                await Task.Run (() => instance.Handle(message));
            }
        }
       
        public async Task RaiseEventAsync<T>(T theEvent) where T : Event
        {
            var handlers = RegisteredHandlers.Where(handler => handler.Key == theEvent.GetType());
            foreach (var handler in handlers)
            {
                var instance = (ICanHandle<T>)_container.GetService(handler.Key);
                await Task.Run(() => instance.Handle(theEvent));
            }
        }       
    }
}
