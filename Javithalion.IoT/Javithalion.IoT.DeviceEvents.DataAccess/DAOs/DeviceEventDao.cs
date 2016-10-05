using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Javithalion.IoT.DeviceEvents.Domain.Entities;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Javithalion.IoT.DeviceEvents.DataAccess.DAOs
{
    public class DeviceEventDao : IDeviceEventDao
    {
        private const string CollectionName = "DeviceEvents";
        private readonly IMongoDatabase _documentStore;

        public DeviceEventDao(IMongoDatabase mongoDatabase)
        {
            _documentStore = mongoDatabase;

            _documentStore.GetCollection<DeviceEvent>(CollectionName).InsertOne(new DeviceEvent() { Date = DateTime.Now });
        }

        public async Task<IList<DeviceEvent>> FindAll()
        {
            var collection = _documentStore.GetCollection<DeviceEvent>(CollectionName);
            return await collection.Find(new BsonDocument()).ToListAsync();
        }
    }
}
