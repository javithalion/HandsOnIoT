using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Javithalion.IoT.DeviceEvents.Domain.Entities;
using Javithalion.IoT.DeviceEvents.DataAccess.DAOs;
using Javithalion.IoT.DeviceEvents.DataAccess.Extensions;
using Javithalion.IoT.DeviceEvents.Business.ReadModel.DTOs;
using AutoMapper;
using System.Linq;
using MongoDB.Driver;

namespace Javithalion.IoT.DeviceEvents.Business.ReadModel
{
    public class DeviceEventReadService : IDeviceEventReadService
    {
        private readonly IDeviceEventDao _deviceEventDao;
        private readonly IMapper _mapper;

        public DeviceEventReadService(IDeviceEventDao deviceEventDao, IMapper mapper)
        {
            _deviceEventDao = deviceEventDao;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DeviceEventDto>> FindAllForDeviceAsync(Guid deviceId)
        {
            var events = await _deviceEventDao.AllDeviceEvents()
                                              .OfDevice(deviceId)
                                              .CurrentlyActive()
                                              .ToListAsync();

            events = GetDummyEvents(deviceId).Union(GetDummyEvents(Guid.NewGuid())).Union(GetDummyEvents(Guid.NewGuid())).ToList();

            return events.Select(@event => _mapper.Map<DeviceEventDto>(@event));
        }

        public async Task<DeviceEventDto> GetAsync(Guid eventId)
        {
            var queryResult = await _deviceEventDao.AllDeviceEvents()
                                                   .WithEventId(eventId)
                                                   .FirstOrDefaultAsync();

            return _mapper.Map<DeviceEventDto>(queryResult);
        }


        private IEnumerable<DeviceEvent> GetDummyEvents(Guid deviceId)
        {
            var result = new List<DeviceEvent>();
            int numberOfDummyEvents = 300;

            for (int i = 0; i < numberOfDummyEvents; i++)
            {
                result.Add(DeviceEvent.NewStartUpEvent(deviceId));
            }
            return result;
        }
    }
}
