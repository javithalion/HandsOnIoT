using Javithalion.IoT.DeviceEvents.Business.ReadModel.DTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
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

        [Fact]
        public async Task FindAll_Ok()
        {
            var response = await _client.GetAsync(@"/api/DeviceEvents?deviceId" + Guid.NewGuid());
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
        }

        [Fact]
        public async Task GetOne_Ok()
        {
            var response = await _client.GetAsync(@"/api/DeviceEvents/" + Guid.NewGuid());
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
        }
    }
}
