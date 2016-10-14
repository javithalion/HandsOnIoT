using Javithalion.IoT.Infraestructure.Bus.BusEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.DeviceEvents.Business.WriteModel.Commands
{
    public class DeleteDeviceEventCommand : Command
    {
        [Required]
        public Guid EventId { get; set; }
    }
}
