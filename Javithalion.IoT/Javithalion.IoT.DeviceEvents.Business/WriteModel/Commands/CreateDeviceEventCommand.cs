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

        [Required(AllowEmptyStrings = true)]
        public string TypeName { get; set; }

        public dynamic Details { get; set; }
    }

    public enum EventType
    {
        StartUp,
        Shutdown,
        PerformanceOverview,
        PerformanceDetails,
        Others
    }
}
