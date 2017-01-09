using Javithalion.IoT.DeviceEvents.Business.ReadModel.DTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Javithalion.IoT.DeviceEvents.Service.IntegrationTests.WriteRequests
{
    public class ManipulateDeviceEventTestFixtures
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public ManipulateDeviceEventTestFixtures()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>()
                                                         .UseEnvironment("Development")
                                                         .UseContentRoot(Directory.GetCurrentDirectory()));
            _client = _server.CreateClient();
        }

        [Fact(DisplayName = "CreateOne_Ok")]
        [Trait("Category", "DeviceEvents.Integration.Write")]
        public async Task CreateOne_Ok()
        {
            //TODO :: Try the "multiple test values" on this test to create all event types.
            var deviceId = Guid.NewGuid();
            var eventType = 0;
            var typeName = string.Empty;

            var requestContent = new StringContent($@"{{
                                                      'deviceId': '{deviceId}',
                                                      'eventType': {eventType},
                                                      'typeName': '{typeName}'
                                                     }}",
                                                     Encoding.UTF8,
                                                     @"application/json");

            var response = await _client.PostAsync(@"/api/DeviceEvents", requestContent);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Created);

            var responseContent = await response.Content.ReadAsStringAsync();
            var parsedResponse = JsonConvert.DeserializeObject<DeviceEventDto>(responseContent);

            Assert.NotNull(parsedResponse);
            Assert.True(parsedResponse.Id != Guid.Empty);
            Assert.True(parsedResponse.Date != DateTime.MinValue);
            Assert.True(parsedResponse.Details.ToString() == string.Empty);
            Assert.True(parsedResponse.DeviceId == deviceId);
            Assert.True(parsedResponse.Type == "StartUp");
        }

        [Fact(DisplayName = "CreateOne_WrongData")]
        [Trait("Category", "DeviceEvents.Integration.Write")]
        public async Task CreateOne_WrongData()
        {
            //TODO :: Try the "multiple test values" on this test to create multiple wrong data.
            var deviceId = "adadasd";
            var eventType = 9;
            var typeName = "adasdsad";

            var requestContent = new StringContent($@"{{
                                                      'deviceId': '{deviceId}',
                                                      'eventType': {eventType},
                                                      'typeName': '{typeName}'
                                                     }}",
                                                     Encoding.UTF8,
                                                     @"application/json");

            var response = await _client.PostAsync(@"/api/DeviceEvents", requestContent);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains($"The input was not valid", responseContent);
        }

        [Fact(DisplayName = "UpdateOne_Ok")]
        [Trait("Category", "DeviceEvents.Integration.Write")]
        public async Task UpdateOne_Ok()
        {
            //TODO :: Try the "multiple test values" on this test to modify with different details values
            var eventId = new Guid("0989b655-b0ae-413c-8503-a6e900f04e3a");
            dynamic details = new { Data = "Hou Hou Hou" };

            var requestContent = new StringContent($@"{{
                                                      'EventId': '{eventId}',                                                      
                                                      'details': {JsonConvert.SerializeObject(details)}
                                                     }}",
                                                     Encoding.UTF8,
                                                     @"application/json");

            var response = await _client.PutAsync(@"/api/DeviceEvents", requestContent);
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.True(responseContent == string.Empty);
        }

        [Fact(DisplayName = "UpdateOne_WrongData")]
        [Trait("Category", "DeviceEvents.Integration.Write")]
        public async Task UpdateOne_WrongData()
        {
            var eventId = "adasdasd";
            var requestContent = new StringContent($@"{{
                                                        'EventId': '{eventId}',
                                                        'Details': {{}}
                                                      }}",
                                                    Encoding.UTF8,
                                                    @"application/json");

            var response = await _client.PutAsync(@"/api/DeviceEvents", requestContent);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.Contains($"The input was not valid", responseContent);
        }

        [Fact(DisplayName = "UpdateOne_NonExistingDeviceEvent")]
        [Trait("Category", "DeviceEvents.Integration.Write")]
        public async Task UpdateOne_NonExistingDeviceEvent()
        {
            var eventId = Guid.NewGuid();
            var requestContent = new StringContent($@"{{
                                                        'EventId': '{eventId}',
                                                        'Details': {{}}
                                                      }}",
                                                    Encoding.UTF8,
                                                    @"application/json");

            var response = await _client.PutAsync(@"/api/DeviceEvents", requestContent);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.NotFound);
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.Contains($"{eventId} not found", responseContent);
        }

        [Fact(DisplayName = "DeleteOne_Ok")]
        [Trait("Category", "DeviceEvents.Integration.Write")]
        public async Task DeleteOne_Ok()
        {
            var eventId = new Guid("0989b655-b0ae-413c-8503-a6e900f04e3a");

            var response = await _client.DeleteAsync(@"/api/DeviceEvents/" + eventId);
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.True(responseContent == string.Empty);
        }

        [Fact(DisplayName = "DeleteOne_WrongData")]
        [Trait("Category", "DeviceEvents.Integration.Write")]
        public async Task DeleteOne_WrongData()
        {
            var eventId = "sadsadsad";
            var response = await _client.DeleteAsync(@"/api/DeviceEvents/" + eventId);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.Contains($"The value '{eventId}' is not valid for Guid", responseContent);
        }

        [Fact(DisplayName = "DeleteOne_NonExistingDeviceEvent")]
        [Trait("Category", "DeviceEvents.Integration.Write")]
        public async Task DeleteOne_NonExistingDeviceEvent()
        {
            var eventId = Guid.NewGuid();
            var response = await _client.DeleteAsync(@"/api/DeviceEvents/" + eventId);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.NotFound);
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.Contains($"{eventId} not found", responseContent);
        }
    }
}
