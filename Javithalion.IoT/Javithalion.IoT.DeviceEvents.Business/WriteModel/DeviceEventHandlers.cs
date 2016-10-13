using AutoMapper;
using Javithalion.IoT.DeviceEvents.Business.WriteModel.Commands;
using Javithalion.IoT.DeviceEvents.DataAccess.DAOs;
using Javithalion.IoT.Infraestructure.Bus.BusEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.DeviceEvents.Business.WriteModel
{
    public class DeviceEventHandlers :
        ICanHandle<CreateDeviceEventCommand>,
        ICanHandle<UpdateDeviceEventCommand>,
        ICanHandle<DeleteDeviceEventCommand>
    {
        private readonly IDeviceEventDao _deviceEventServiceDao;
        private readonly IMapper _mapper;

        public DeviceEventHandlers(IDeviceEventDao deviceEventDao, IMapper mapper)
        {
            _deviceEventServiceDao = deviceEventDao;
            _mapper = mapper;
        }

        public Task Handle(CreateDeviceEventCommand message)
        {
            throw new NotImplementedException();
        }

        public Task Handle(UpdateDeviceEventCommand message)
        {
            throw new NotImplementedException();
        }

        public Task Handle(DeleteDeviceEventCommand message)
        {
            throw new NotImplementedException();
        }
    }
}
