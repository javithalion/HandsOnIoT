using AutoMapper;
using Javithalion.IoT.DeviceEvents.Business.WriteModel.Commands;
using Javithalion.IoT.DeviceEvents.Business.WriteModel.Events;
using Javithalion.IoT.DeviceEvents.DataAccess.DAOs;
using Javithalion.IoT.DeviceEvents.Domain.Entities;
using Javithalion.IoT.Infraestructure.Bus.BusEntities;
using Javithalion.IoT.Infraestructure.ModelBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.DeviceEvents.Business.WriteModel
{
    public class DeviceEventHandlers :
        ICanHandle<CreateDeviceEventCommand>,
        ICanHandle<UpdateDeviceEventCommand>,
        ICanHandle<DeleteDeviceEventCommand>,
        ICanHandle<ExisitingDeviceEventModifiedEvent>,
        ICanHandle<ExistingDeviceEventDeletedEvent>,
        ICanHandle<NewDeviceEventAddedEvent>
    {
        private readonly IServiceBus _serviceBus;
        private readonly IDeviceEventDao _deviceEventServiceDao;
        private readonly IMapper _mapper;

        public DeviceEventHandlers(IServiceBus serviceBus,IDeviceEventDao deviceEventDao, IMapper mapper)
        {
            _deviceEventServiceDao = deviceEventDao;
            _mapper = mapper;
            _serviceBus = serviceBus;
        }

        public async Task Handle(CreateDeviceEventCommand message)
        {
            //var newEvent = DeviceEvent.CreateNewForDevice(message.DeviceId)
            //                          .OfType(message.EventType.ToString());

            //_deviceEventServiceDao.Insert(newEvent);

            //_serviceBus.Publish(NewDeviceEventAddedEvent.CreateFor(newEvent));
        }

        public Task Handle(ExisitingDeviceEventModifiedEvent message)
        {
            throw new NotImplementedException();
        }

        public Task Handle(UpdateDeviceEventCommand message)
        {
            throw new NotImplementedException();
        }

        public Task Handle(NewDeviceEventAddedEvent message)
        {
            throw new NotImplementedException();
        }

        public Task Handle(ExistingDeviceEventDeletedEvent message)
        {
            throw new NotImplementedException();
        }

        public Task Handle(DeleteDeviceEventCommand message)
        {
            throw new NotImplementedException();
        }
    }
}
