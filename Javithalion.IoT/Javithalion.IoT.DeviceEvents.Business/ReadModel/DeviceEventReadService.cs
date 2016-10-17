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

        public async Task<IEnumerable<DeviceEventDto>> FindAllForDeviceAsync(string deviceId, int page, int pageSize)
        {
            if (string.IsNullOrEmpty(deviceId))
                throw new ArgumentException("Null or empty device id was provided", nameof(deviceId));

            Guid parsedDeviceGuid;
            if (!Guid.TryParse(deviceId, out parsedDeviceGuid))
                throw new ArgumentException("Provided device id has incorrect format", nameof(deviceId));

            var events = await _deviceEventDao.AllDeviceEvents()
                                                     .OfDevice(parsedDeviceGuid)
                                                     .CurrentlyActive()
                                                     .Paged(page, pageSize)
                                                     .ToListAsync();

            return events.Select(@event => _mapper.Map<DeviceEventDto>(@event));
        }

        public async Task<DeviceEventDto> GetAsync(string eventId)
        {
            if (string.IsNullOrEmpty(eventId))
                throw new ArgumentException("Null or empty event id was provided", nameof(eventId));

            Guid parsedEventGuid;
            if (!Guid.TryParse(eventId, out parsedEventGuid))
                throw new ArgumentException("Provided event id has incorrect format", nameof(eventId));


            var queryResult = await _deviceEventDao.AllDeviceEvents()
                                                          .WithEventId(parsedEventGuid)
                                                          .FirstOrDefaultAsync();

            return _mapper.Map<DeviceEventDto>(queryResult);
        }
    }
}
