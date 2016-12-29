using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Mongo2Go;
using System.IO;
using MongoDB.Driver;
using Javithalion.IoT.DeviceEvents.DataAccess.DAOs;
using Javithalion.IoT.DeviceEvents.DataAccess.Extensions;
using System;
using Microsoft.Extensions.Configuration;
using Javithalion.IoT.DeviceEvents.Domain.Entities;

namespace Javithalion.IoT.DeviceEvents.DataAccess.Tests.ExtensionTestFixtures
{
    public class DeviceEventDataAccessExtensionsTestFixture
    {        
        private readonly IMongoDatabase _database;

        public DeviceEventDataAccessExtensionsTestFixture()
        {
            IConfigurationRoot configuration = GetConfiguration();
            _database = GetInMemoryMongoDatabase(configuration);
        }

        private static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();
            return configuration;
        }

        private IMongoDatabase GetInMemoryMongoDatabase(IConfigurationRoot configuration)
        {
            var runner = MongoDbRunner.StartForDebugging();

            var seedingFilePath = Path.Combine(Directory.GetCurrentDirectory(), configuration["DeviceEventDatabase:SeedingFile"]);
            runner.Import(configuration["DeviceEventDatabase:Name"],
                         configuration["DeviceEventDatabase:CollectionName"],
                         seedingFilePath,
                         true);

            var client = new MongoClient(new MongoClientSettings
            {
                MaxConnectionPoolSize = 800,
                Server = new MongoServerAddress("localhost", 27017)
            });

            return client.GetDatabase(configuration["DeviceEventDatabase:Name"]);
        }

        [Fact(DisplayName = "FindAll_CurrentlyActive")]
        [Trait("Category", "DeviceEvents.DataAccess.Extensions")]
        public async Task FindAll_Ok()
        {
            var dao = new DeviceEventDao(_database);

            var result = await dao.AllDeviceEvents()
                                  .ToListAsync();

            Assert.True(result.Count() == 20); // See deviceEventsSeedingData.json           
        }



        [Fact(DisplayName = "FindAll_CurrentlyActive")]
        [Trait("Category", "DeviceEvents.DataAccess.Extensions")]
        public async Task FindAll_CurrentlyActive()
        {
            var dao = new DeviceEventDao(_database);

            var result = await dao.AllDeviceEvents()
                                  .CurrentlyActive()
                                  .ToListAsync();

            Assert.True(result.Count() == 17); // See deviceEventsSeedingData.json
            Assert.True(result.All(x => !x.Deleted));
        }

        [Fact(DisplayName = "FindAll_ForDevice")]
        [Trait("Category", "DeviceEvents.DataAccess.Extensions")]
        public async Task FindAll_ForExisitingDevice()
        {
            var deviceId = new Guid("95004417-1492-447a-aaf2-d6a1a49c4d69");
            var dao = new DeviceEventDao(_database);

            var result = await dao.AllDeviceEvents()
                                  .ForDevice(deviceId)
                                  .ToListAsync();

            Assert.True(result.Count() == 20); // See deviceEventsSeedingData.json
            Assert.True(result.All(x => x.DeviceId == deviceId));
        }

        [Fact(DisplayName = "FindAll_ForNonExisitingDevice")]
        [Trait("Category", "DeviceEvents.DataAccess.Extensions")]
        public async Task FindAll_ForNonExisitingDevice()
        {
            var deviceId = Guid.Empty;
            var dao = new DeviceEventDao(_database);

            var result = await dao.AllDeviceEvents()
                                  .ForDevice(deviceId)
                                  .ToListAsync();

            Assert.True(result.Count() == 0); // See deviceEventsSeedingData.json
        }

        [Fact(DisplayName = "FindAll_Paged")]
        [Trait("Category", "DeviceEvents.DataAccess.Extensions")]
        public async Task FindAll_Paged()
        {
            var expectedPage = 2;
            var pageSize = 5;
            var expectedData = Enumerable.Range(expectedPage * pageSize, pageSize);

            var dao = new DeviceEventDao(_database);

            var result = await dao.AllDeviceEvents()
                                  .Paged(expectedPage, pageSize)
                                  .ToListAsync();

            Assert.True(result.Count() == pageSize); // See deviceEventsSeedingData.json
            Assert.All(result, x => expectedData.ToList().Contains(int.Parse(x.Details.Data)));
        }

        [Fact(DisplayName = "FindOne_ById")]
        [Trait("Category", "DeviceEvents.DataAccess.Extensions")]
        public async Task FindOne_ById()
        {
            var eventId = new Guid("0989b655-b0ae-413c-8503-a6e900f04e3a");
            var deviceId = new Guid("95004417-1492-447a-aaf2-d6a1a49c4d69");

            var dao = new DeviceEventDao(_database);

            var result = await dao.AllDeviceEvents()
                                  .WithEventId(eventId)
                                  .FirstAsync();

            Assert.NotNull(result); // See deviceEventsSeedingData.json

            var originalDate = DateTime.Parse("2016-12-27T14:34:54.883Z", null, System.Globalization.DateTimeStyles.RoundtripKind);
            var originalDateWithoutMilliseconds = originalDate.AddMilliseconds(-originalDate.Millisecond);
            var parsedDateWithoutMilliseconds = result.Date.AddMilliseconds(-result.Date.Millisecond);

            Assert.True(result.Id == eventId);
            Assert.True(result.DeviceId == deviceId);
            Assert.True(result.TypeName == "Custom");
            Assert.True(result.Type == EventType.Others);
            Assert.True(originalDateWithoutMilliseconds == parsedDateWithoutMilliseconds);
            Assert.NotNull(result.Details);
        }

    }
}
