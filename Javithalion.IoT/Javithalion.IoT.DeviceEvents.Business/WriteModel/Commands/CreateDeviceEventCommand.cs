using Javithalion.IoT.Infraestructure.Bus.BusEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Javithalion.IoT.DeviceEvents.Business.WriteModel.Commands
{
    public class CreateDeviceEventCommand : Command
    {
        [Required]
        public Guid DeviceId { get; set; }

        [Required]
        public EventType EventType { get; set; }
    }

    public enum EventType
    {
        StartUp,
        Shutdown,
        Performance
    }
}
