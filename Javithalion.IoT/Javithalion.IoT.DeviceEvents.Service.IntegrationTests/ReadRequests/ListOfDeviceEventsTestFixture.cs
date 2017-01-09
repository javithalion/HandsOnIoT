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
    }
}
