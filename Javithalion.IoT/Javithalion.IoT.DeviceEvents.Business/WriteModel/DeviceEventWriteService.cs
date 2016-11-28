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
            var newEvent = CreateDeviceEventFromCommand(createCommand);

            await _deviceEventServiceDao.InsertAsync(newEvent);
            return _mapper.Map<DeviceEvent, DeviceEventDto>(newEvent);
        }

        private DeviceEvent CreateDeviceEventFromCommand(CreateDeviceEventCommand createCommand)
        {
            switch(createCommand.EventType)
            {
                case Commands.EventType.StartUp:
                    return DeviceEvent.NewStartUpEvent(createCommand.DeviceId);
                case Commands.EventType.Shutdown:
                    return DeviceEvent.NewTearDownEvent(createCommand.DeviceId);
                case Commands.EventType.PerformanceOverview:
                    return DeviceEvent.NewResourcesOverviewEvent(createCommand.DeviceId, createCommand.Details);
                case Commands.EventType.PerformanceDetails:
                    return DeviceEvent.NewResourcesDetailedEvent(createCommand.DeviceId, createCommand.Details);
                default:
                    return DeviceEvent.NewCustomEvent(createCommand.DeviceId, createCommand.TypeName, createCommand.Details);
            }
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
                theEvent.WithDetails(updateCommand.Details);                     
                        
                await _deviceEventServiceDao.UpdateAsync(theEvent);
                return _mapper.Map<DeviceEvent, DeviceEventDto>(theEvent);
            }
        }
    }
}
