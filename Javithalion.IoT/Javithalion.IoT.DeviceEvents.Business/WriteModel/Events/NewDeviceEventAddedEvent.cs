using Javithalion.IoT.Infraestructure.Bus.BusEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Javithalion.IoT.DeviceEvents.Domain.Entities;

namespace Javithalion.IoT.DeviceEvents.Business.WriteModel.Events
{
    public class NewDeviceEventAddedEvent : Event
    {
        public DeviceEvent TheEvent { get; private set; }

        private NewDeviceEventAddedEvent() { }

        public static NewDeviceEventAddedEvent CreateFor(DeviceEvent newEvent)
        {
            return new NewDeviceEventAddedEvent()
            {
                TimeStamp = DateTime.Now,
                TheEvent = newEvent
            };
        }
    }
}
