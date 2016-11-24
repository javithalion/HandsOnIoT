using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Javithalion.IoT.DeviceEvents.Domain.Entities;
using Javithalion.IoT.DeviceEvents.DataAccess.DAOs;
using Javithalion.IoT.DeviceEvents.DataAccess.Extensions;
using MongoDB.Driver;
using Javithalion.IoT.DeviceEvents.Business.ReadModel.DTOs;
using AutoMapper;
using System.Linq;

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
            //var events = await _deviceEventDao.AllDeviceEvents()
            //                                         .OfDevice(parsedDeviceGuid)
            //                                         .CurrentlyActive()
            //                                         .ToListAsync();

            var events = GetDummyEvents(deviceId);

            return events.Select(@event => _mapper.Map<DeviceEventDto>(@event));
        }

        private IEnumerable<DeviceEvent> GetDummyEvents(Guid deviceId)
        {
            var result = new List<DeviceEvent>();
            int numberOfDummyEvents = 300;

            for(int i = 0;i< numberOfDummyEvents; i++)
            {
                result.Add(DeviceEvent.CreateNewForDevice(deviceId).OfType("Dummy"));
            }
            return result;
        }

        public async Task<DeviceEventDto> GetAsync(Guid eventId)
        {
            var queryResult = await _deviceEventDao.AllDeviceEvents()
                                                          .WithEventId(eventId)
                                                          .FirstOrDefaultAsync();

            return _mapper.Map<DeviceEventDto>(queryResult);
        }
    }
}
