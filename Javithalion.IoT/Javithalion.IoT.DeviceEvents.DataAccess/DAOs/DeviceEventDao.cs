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

        public async Task InsertAsync(DeviceEvent deviceEvent)
        {
            await _documentStore.GetCollection<DeviceEvent>(CollectionName).InsertOneAsync(deviceEvent);
        }

        public async Task UpdateAsync(DeviceEvent theEvent)
        {
            var filter = Builders<DeviceEvent>.Filter.Where(x => x.Id == theEvent.Id);

            var update = Builders<DeviceEvent>.Update.Set(x => x.Deleted, theEvent.Deleted)
                                                     .Set(x => x.Type, theEvent.Type)
                                                     .Set(x => x.Date, theEvent.Date)
                                                     .Set(x => x.DeviceId, theEvent.DeviceId);                                                     

            var result = await _documentStore.GetCollection<DeviceEvent>(CollectionName).UpdateOneAsync(filter, update);
        }
    }
}
