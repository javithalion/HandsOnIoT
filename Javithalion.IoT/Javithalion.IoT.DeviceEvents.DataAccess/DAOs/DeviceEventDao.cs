using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Javithalion.IoT.DeviceEvents.Domain.Entities;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Linq;

namespace Javithalion.IoT.DeviceEvents.DataAccess.DAOs
{
    public class DeviceEventDao : IDeviceEventDao
    {
        private const string CollectionName = "DeviceEventCollection";
        private readonly IMongoDatabase _documentStore;              

        public DeviceEventDao(IMongoDatabase mongoDatabase)
        {
            _documentStore = mongoDatabase;
        }

        public IMongoQueryable<DeviceEvent> AllDeviceEvents()
        {
            return _documentStore.GetCollection<DeviceEvent>(CollectionName).AsQueryable<DeviceEvent>();
        }
    }
}
