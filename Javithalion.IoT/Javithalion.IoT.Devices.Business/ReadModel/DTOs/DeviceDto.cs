﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.Devices.Business.ReadModel.DTOs
{
    public class DeviceDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string IpAddress { get; set; }

        public int OperativeSystemCode { get; set; }

        public string OperativeSystemName { get; set; }
    }
}
