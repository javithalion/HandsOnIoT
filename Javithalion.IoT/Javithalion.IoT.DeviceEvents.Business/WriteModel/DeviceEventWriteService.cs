using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Javithalion.IoT.DeviceEvents.Domain.Entities;
using Javithalion.IoT.DeviceEvents.Business.WriteModel.Commands;
using Javithalion.IoT.DeviceEvents.Business.ReadModel.DTOs;
using Javithalion.IoT.DeviceEvents.DataAccess.DAOs;
using AutoMapper;
using MongoDB.Driver;

namespace Javithalion.IoT.DeviceEvents.Business.WriteModel
{
    public class DeviceEventWriteService : IDeviceEventWriteService
    {
        private readonly IDeviceEventDao _deviceEventServiceDao;
        private readonly IMapper _mapper;

        public DeviceEventWriteService(IDeviceEventDao deviceEventDao, IMapper mapper)
        {
            _deviceEventServiceDao = deviceEventDao;
            _mapper = mapper;
        }

        public async Task<DeviceEventDto> AddDeviceEventAsync(CreateDeviceEventCommand createCommand)
        {
            var deviceEvent = DeviceEvent.CreateNewForDevice(createCommand.DeviceId)
                                         .OfType(createCommand.EventType.ToString());

            _deviceEventServiceDao.AllDeviceEvents().

        }

        public async Task RemoveDeviceEventFromDeviceAsync(string deviceId, string eventId)
        {
            throw new NotImplementedException();
        }
    }
}
