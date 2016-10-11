using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.DeviceEvents.Business.WriteModel.Commands
{
    public class CreateDeviceEventCommand
    {
        public Guid DeviceId { get; set; }

        public EventType EventType { get; set; }
    }

    public enum EventType
    {
        StartUp,
        Shutdown,
        Performance
    }
}
