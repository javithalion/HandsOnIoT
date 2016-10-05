using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Javithalion.IoT.DeviceEvents.Domain.Entities;
using Javithalion.IoT.DeviceEvents.DataAccess.DAOs;

namespace Javithalion.IoT.DeviceEvents.Business.ReadModel
{
    public class DeviceEventReadService : IDeviceEventReadService
    {
        private readonly IDeviceEventDao _deviceEventServiceDao;

        public DeviceEventReadService(IDeviceEventDao deviceEventDao)
        {
            _deviceEventServiceDao = deviceEventDao;
        }

        public async Task<IList<DeviceEvent>> FindAllAsync()
        {
            return await _deviceEventServiceDao.FindAll();
        }
    }
}
