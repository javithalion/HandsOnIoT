using Javithalion.IoT.DeviceEvents.Business.ReadModel.DTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Javithalion.IoT.DeviceEvents.Service.IntegrationTests.ReadRequests
{
    public class ListOfDeviceEventsTestFixture
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public ListOfDeviceEventsTestFixture()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>()
                                                         .UseEnvironment("Development")
                                                         .UseContentRoot(Directory.GetCurrentDirectory()));
            _client = _server.CreateClient();
        }

        [Fact(DisplayName = "FindAll_Ok")]
        [Trait("Category", "DeviceEvents.Integration.Read")]
        public async Task FindAll_Ok()
        {
            var deviceId = new Guid("95004417-1492-447a-aaf2-d6a1a49c4d69"); // see deviceEventsSeedingData.json file

            var response = await _client.GetAsync(@"/api/DeviceEvents?deviceId=" + deviceId);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var parsedResponse = JsonConvert.DeserializeObject<IList<DeviceEventDto>>(responseContent);

            Assert.True(parsedResponse.Count == 20);
            Assert.True(parsedResponse.All(x => x.DeviceId == deviceId));
        }

        [Fact(DisplayName = "FindAll_NonExisitngDeviceId")]
        [Trait("Category", "DeviceEvents.Integration.Read")]
        public async Task FindAll_NonExisitngDeviceId()
        {
            var response = await _client.GetAsync(@"/api/DeviceEvents?deviceId=" + Guid.Empty);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var parsedResponse = JsonConvert.DeserializeObject<IList<DeviceEventDto>>(responseContent);

            Assert.True(parsedResponse.Count == 0); // see deviceEventsSeedingData.json file           
        }

        [Fact(DisplayName = "FindAll_NotProvidingDeviceId")]
        [Trait("Category", "DeviceEvents.Integration.Read")]
        public async Task FindAll_NotProvidingDeviceId()
        {
            var response = await _client.GetAsync(@"/api/DeviceEvents?deviceId=");
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("invalid", responseContent);
        }

        [Fact(DisplayName = "GetOne_Ok")]
        [Trait("Category", "DeviceEvents.Integration.Read")]
        public async Task GetOne_Ok()
        {
            var eventId = new Guid("0989b655-b0ae-413c-8503-a6e900f04e3a");
            var deviceId = new Guid("95004417-1492-447a-aaf2-d6a1a49c4d69"); // see deviceEventsSeedingData.json file   

            var response = await _client.GetAsync(@"/api/DeviceEvents/" + eventId); // see collection with MongoChef, based on deviceEventsSeedingData.json file
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var parsedResponse = JsonConvert.DeserializeObject<DeviceEventDto>(responseContent);

            var originalDate = DateTime.Parse("2016-12-27T14:34:54.883Z", null, System.Globalization.DateTimeStyles.RoundtripKind);
            var originalDateWithoutMilliseconds = originalDate.AddMilliseconds(-originalDate.Millisecond);
            var parsedDateWithoutMilliseconds = parsedResponse.Date.AddMilliseconds(-parsedResponse.Date.Millisecond);

            Assert.True(parsedResponse.Id == eventId);
            Assert.True(parsedResponse.DeviceId == deviceId);
            Assert.True(parsedResponse.Type == "Custom");
            Assert.True(originalDateWithoutMilliseconds == parsedDateWithoutMilliseconds);
            Assert.NotNull(parsedResponse.Details);
        }
    }
}
