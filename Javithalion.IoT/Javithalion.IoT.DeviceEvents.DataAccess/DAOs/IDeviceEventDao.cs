using Javithalion.IoT.DeviceEvents.Domain.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.DeviceEvents.DataAccess.DAOs
{
    public interface IDeviceEventDao
    {
        IMongoQueryable<DeviceEvent> AllDeviceEvents();
        Task Insert(DeviceEvent deviceEvent);
    }
}