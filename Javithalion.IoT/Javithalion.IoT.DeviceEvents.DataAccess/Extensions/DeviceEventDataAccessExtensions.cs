using Javithalion.IoT.DeviceEvents.Domain.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Linq;

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
            return collection.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public static IMongoQueryable<DeviceEvent> ForDevice(this IMongoQueryable<DeviceEvent> collection, Guid deviceId)
        {
            return collection.Where(x => x.DeviceId == deviceId);
        }

        public static IMongoQueryable<DeviceEvent> FromYesterday(this IMongoQueryable<DeviceEvent> collection)
        {
            var fromDate = DateTime.Now.Date.AddDays(-1);
            var toDate = DateTime.Now.Date;

            return collection.BetweenDates(fromDate, toDate);
        }

        public static IMongoQueryable<DeviceEvent> BetweenDates(this IMongoQueryable<DeviceEvent> collection, DateTime from, DateTime to)
        {
            return collection.Where(x => x.Date >= from && x.Date <= to);
        }

        public static IMongoQueryable<DeviceEvent> WithEventId(this IMongoQueryable<DeviceEvent> collection, Guid eventId)
        {
            return collection.Where(x => x.Id == eventId);
        }
    }
}
