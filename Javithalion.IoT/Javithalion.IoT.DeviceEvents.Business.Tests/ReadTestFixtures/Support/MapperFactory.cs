using AutoMapper;
using Javithalion.IoT.DeviceEvents.Business.ReadModel.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.DeviceEvents.Business.Tests.ReadTestFixtures.Support
{
    public static class MapperFactory
    {
        public static IMapper GetReaderServiceMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfileConfiguration());
            });

            return mapperConfiguration.CreateMapper();
        }
    }
}
