using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Javithalion.IoT.Devices.Business.ReadModel.DTOs;
using Javithalion.IoT.Devices.DataAccess.Read;
using AutoMapper;
using Javithalion.IoT.Devices.Domain.Entities;

namespace Javithalion.IoT.Devices.Business.ReadModel
{
    public class DeviceReadService : IDeviceReadService
    {
        private readonly IDeviceDao _deviceDao;
        private readonly IMapper _mapper;

        public DeviceReadService(IDeviceDao deviceEventDao, IMapper mapper)
        {
            _deviceDao = deviceEventDao;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DeviceDto>> FindAllAsync()
        {
            var results = await _deviceDao.FindAllAsync();
            return results.Select(x => _mapper.Map<Device, DeviceDto>(x));
        }

        public async Task<DeviceDto> GetAsync(Guid id)
        {
            var result = await _deviceDao.GetAsync(id);
            return _mapper.Map<Device, DeviceDto>(result);
        }
    }
}
