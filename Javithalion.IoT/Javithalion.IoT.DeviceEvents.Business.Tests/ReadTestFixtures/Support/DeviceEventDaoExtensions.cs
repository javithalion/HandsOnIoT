using Javithalion.IoT.DeviceEvents.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.DeviceEvents.Business.Tests.ReadTestFixtures.Support
{
    public static class DeviceEventDaoExtensions
    {
        public static IMongoQueryable<DeviceEvent> AsMongoQueryable(this IQueryable<DeviceEvent> collection)
        {
            var client = new MongoClient("mongodb://localhost"); 
            var database = client.GetDatabase("test");            

            var testCollection = database.GetCollection<DeviceEvent>("testCollection");
            testCollection.InsertMany(collection);

            return testCollection.AsQueryable();
        }
    }
}
