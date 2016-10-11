using Javithalion.IoT.DeviceEvents.Domain.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.DeviceEvents.DataAccess.Extensions
{
    public static class DeviceEventDataAccessExtensions
    {
        public static IMongoQueryable<DeviceEvent> CurrentlyActive(this IMongoQueryable<DeviceEvent> collection)
        {
            return collection.Where(x => !x.Deleted);
        }

        public static IMongoQueryable<DeviceEvent> Paged(this IMongoQueryable<DeviceEvent> collection, int page = 1, int pageSize = 50)
        {
            return collection.Skip((page-1)*pageSize).Take(pageSize);
        }

        public static IMongoQueryable<DeviceEvent> OfDevice(this IMongoQueryable<DeviceEvent> collection, Guid deviceId)
        {
            return collection.Where(x => x.DeviceId == deviceId);
        }

        public static IMongoQueryable<DeviceEvent> WithEventId(this IMongoQueryable<DeviceEvent> collection, Guid eventId)
        {
            return collection.Where(x => x.Id == eventId);
        }
    }
}
