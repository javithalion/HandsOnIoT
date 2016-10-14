using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Javithalion.IoT.DeviceEvents.Business.ReadModel.DTOs;
using Javithalion.IoT.DeviceEvents.Business.WriteModel.Commands;
using AutoMapper;
using Javithalion.IoT.DeviceEvents.DataAccess.DAOs;
using Javithalion.IoT.DeviceEvents.Domain.Entities;
using Javithalion.IoT.DeviceEvents.DataAccess.Extensions;
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

        public async Task<DeviceEventDto> CreateAsync(CreateDeviceEventCommand createCommand)
        {
            var newEvent = DeviceEvent.CreateNewForDevice(createCommand.DeviceId)
                                     .OfType(createCommand.EventType.ToString());

            await _deviceEventServiceDao.InsertAsync(newEvent);
            return _mapper.Map<DeviceEvent, DeviceEventDto>(newEvent);
        }

        public async Task<DeviceEventDto> DeleteAsync(DeleteDeviceEventCommand deleteCommand)
        {
            var theEvent = await _deviceEventServiceDao.AllDeviceEvents()
                                                       .WithEventId(deleteCommand.EventId)
                                                       .FirstOrDefaultAsync();
            if (theEvent == null)
                throw new KeyNotFoundException();
            else
            {
                theEvent.Disable();
                await _deviceEventServiceDao.UpdateAsync(theEvent);
                return _mapper.Map<DeviceEvent, DeviceEventDto>(theEvent);
            }
        }

        public async Task<DeviceEventDto> UpdateAsync(UpdateDeviceEventCommand updateCommand)
        {
            var theEvent = await _deviceEventServiceDao.AllDeviceEvents()
                                                        .WithEventId(updateCommand.EventId)
                                                        .FirstOrDefaultAsync();
            if (theEvent == null)
                throw new KeyNotFoundException();
            else
            {                
                await _deviceEventServiceDao.UpdateAsync(theEvent);
                return _mapper.Map<DeviceEvent, DeviceEventDto>(theEvent);
            }
        }
    }
}
