using AutoMapper;
using Javithalion.IoT.DeviceEvents.Business.ReadModel.DTOs;
using Javithalion.IoT.DeviceEvents.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.DeviceEvents.Business.ReadModel.Maps
{
    public class AutoMapperProfileConfiguration : Profile
    {
        protected override void Configure()
        {
            CreateMap<DeviceEvent, DeviceEventDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.TypeName))
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details));
        }
    }
}
