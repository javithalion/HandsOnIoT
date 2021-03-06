﻿using Javithalion.IoT.DeviceEvents.Business.ReadModel.DTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Javithalion.IoT.DeviceEvents.Service.IntegrationTests.ReadRequests
{
    public class SingleDeviceEventTestFixture
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public SingleDeviceEventTestFixture()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>()
                                                         .UseEnvironment("Development")
                                                         .UseContentRoot(Directory.GetCurrentDirectory()));
            _client = _server.CreateClient();
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

        [Fact(DisplayName = "GetOne_NonExisting")]
        [Trait("Category", "DeviceEvents.Integration.Read")]
        public async Task GetOne_NonExisting()
        {
            var eventId = Guid.Empty;

            var response = await _client.GetAsync(@"/api/DeviceEvents/" + eventId);
            var responseStatusCode = response.StatusCode;

            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.True(responseStatusCode == System.Net.HttpStatusCode.NotFound);
            Assert.Contains("not found", responseContent);
        }


        [Fact(DisplayName = "GetOne_BadIdentifier")]
        [Trait("Category", "DeviceEvents.Integration.Read")]
        public async Task GetOne_BadIdentifier()
        {
            var eventId = "XXXXXXXXXXXX";

            var response = await _client.GetAsync(@"/api/DeviceEvents/" + eventId);
            var responseStatusCode = response.StatusCode;

            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.True(responseStatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Contains($"value '{eventId}' is not valid for Guid", responseContent);
        }
    }
}
