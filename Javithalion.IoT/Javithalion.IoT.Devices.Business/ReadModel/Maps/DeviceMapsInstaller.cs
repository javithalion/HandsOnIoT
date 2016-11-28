using AutoMapper;
using Javithalion.IoT.Devices.Business.ReadModel.DTOs;
using Javithalion.IoT.Devices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.Devices.Business.ReadModel.Maps
{
    public class DeviceMapsInstaller : Profile
    {
        private const string ValueForUndefinedIpAdress = "";

        protected override void Configure()
        {
            CreateMap<Device, DeviceDto>()
                .ForMember(dest => dest.IpAddress, opt => opt.MapFrom(src => src.IpAddress.ToString() == UndefinedIPAddress.Value.ToString() ? ValueForUndefinedIpAdress : src.IpAddress.ToString()))
                .ForMember(dest => dest.OperativeSystemCode, opt => opt.MapFrom(src => src.OperativeSystem.Id))
                .ForMember(dest => dest.OperativeSystemName, opt => opt.MapFrom(src => src.OperativeSystem.Name));
        }
    }
}
