using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.DeviceEvents.Business.ReadModel.DTOs
{
    public class DeviceEventDto
    {
        public Guid Id { get; set; }

        public Guid DeviceId { get; set; }

        public DateTime Date { get; set; }

        public string Type { get; set; }

        public dynamic Details { get; set; }

    }
}
