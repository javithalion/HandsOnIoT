using Javithalion.IoT.Infraestructure.Bus.BusEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.Devices.Business.WriteModel.Commands
{
    public class CreateDeviceCommand : Command
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int SelectedOperativeSystemId { get; set; }
                
        public string IpAddress { get; set; }
    }
}
