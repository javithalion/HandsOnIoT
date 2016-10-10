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
        private const string CollectionName = "DeviceEventCollection";
        private readonly IMongoDatabase _documentStore;

        private static readonly IDictionary<Guid, List<DeviceEvent>> _tempDocumentStore = new Dictionary<Guid, List<DeviceEvent>>();

        static DeviceEventDao()
        {
            var deviceId = new Guid("987427ba-7019-47e9-aa56-b992022d591b");
            _tempDocumentStore.Add(deviceId, new List<DeviceEvent>()
                                                   {
                                                        new DeviceEvent() { Id = Guid.NewGuid(), DeviceId = deviceId, Date = DateTime.Now },
                                                        new DeviceEvent() { Id = Guid.NewGuid(), DeviceId = deviceId, Date = DateTime.Now },
                                                        new DeviceEvent() { Id = Guid.NewGuid(), DeviceId = deviceId, Date = DateTime.Now },
                                                        new DeviceEvent() { Id = Guid.NewGuid(), DeviceId = deviceId, Date = DateTime.Now }
                                                   });
        }

        public DeviceEventDao(IMongoDatabase mongoDatabase)
        {
            _documentStore = mongoDatabase;
        }

        public async Task<IList<DeviceEvent>> FindAllForDeviceAsync(Guid deviceId)
        {
            //var collection = _documentStore.GetCollection<DeviceEvent>(CollectionName);
            //return await collection.Find(new BsonDocument()).ToListAsync();

            if (_tempDocumentStore.ContainsKey(deviceId))
                return _tempDocumentStore[deviceId];
            else
                return new List<DeviceEvent>();

        }

        public async Task<DeviceEvent> GetAsync(Guid parsedDeviceGuid, Guid parsedEventGuid)
        {
            if (_tempDocumentStore.ContainsKey(parsedDeviceGuid))
                return _tempDocumentStore[parsedDeviceGuid].FirstOrDefault(x => x.Id == parsedEventGuid);
            else
                return null;
        }
    }
}
