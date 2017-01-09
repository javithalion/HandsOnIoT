using Javithalion.IoT.DeviceEvents.Domain.Entities;
using Mongo2Go;
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
            var runner = MongoDbRunner.StartForDebugging();

            //var client = new MongoClient(runner.ConnectionString);
            var client = new MongoClient(new MongoClientSettings
            {
                MaxConnectionPoolSize = 800,
                Server = new MongoServerAddress("localhost", 27017)
            });

            var database = client.GetDatabase("test");

            var testCollection = database.GetCollection<DeviceEvent>("testCollection");
            if (collection.Any())
                testCollection.InsertMany(collection);

            return testCollection.AsQueryable();
        }        
    }
}
