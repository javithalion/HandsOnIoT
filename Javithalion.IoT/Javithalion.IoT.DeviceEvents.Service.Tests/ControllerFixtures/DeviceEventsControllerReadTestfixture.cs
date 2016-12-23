using Javithalion.IoT.DeviceEvents.Business;
using Javithalion.IoT.DeviceEvents.Business.ReadModel;
using Javithalion.IoT.DeviceEvents.Business.ReadModel.DTOs;
using Javithalion.IoT.DeviceEvents.Service.Controllers;
using Javithalion.IoT.DeviceEvents.Service.Tests.Support;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Javithalion.IoT.DeviceEvents.Service.Tests.ControllerFixtures
{
    public class DeviceEventsControllerReadTestfixture
    {

        [Fact]
        public async void FindAll_Ok()
        {
            // Arrange
            var numberOfEvents = 30;
            var deviceId = Guid.NewGuid();
            var listOfDeviceEvents = DeviceEventsDtoFactory.GetListOfDeviceEvents(deviceId, numberOfEvents);

            var readerServiceMock = new Mock<IDeviceEventReadService>(MockBehavior.Strict);
            var writerServiceMock = new Mock<IDeviceEventWriteService>(MockBehavior.Strict);

            readerServiceMock.Setup(f => f.FindAllForDeviceAsync(deviceId)).Returns(Task.FromResult(listOfDeviceEvents));

            var controller = new DeviceEventsController(writerServiceMock.Object, readerServiceMock.Object);

            // Act
            var response = await controller.GetAllForDevice(deviceId);

            // Assert
            var result = Assert.IsType<OkObjectResult>(response);
            Assert.True(result.StatusCode == (int)HttpStatusCode.OK);

            var deviceEventsInformation = Assert.IsType<List<DeviceEventDto>>(result.Value);
            Assert.True(deviceEventsInformation.Count() == numberOfEvents);
            Assert.All(listOfDeviceEvents, deviceEvent => deviceEventsInformation.Contains(deviceEvent));
        }

        [Fact]
        public async void FindAll_EmptyResult()
        {
            // Arrange
            var numberOfEvents = 0;
            var deviceId = Guid.NewGuid();
            var listOfDeviceEvents = DeviceEventsDtoFactory.GetListOfDeviceEvents(deviceId, numberOfEvents);

            var readerServiceMock = new Mock<IDeviceEventReadService>(MockBehavior.Strict);
            var writerServiceMock = new Mock<IDeviceEventWriteService>(MockBehavior.Strict);

            readerServiceMock.Setup(f => f.FindAllForDeviceAsync(deviceId)).Returns(Task.FromResult(listOfDeviceEvents));

            var controller = new DeviceEventsController(writerServiceMock.Object, readerServiceMock.Object);

            // Act
            var response = await controller.GetAllForDevice(deviceId);

            // Assert
            var result = Assert.IsType<OkObjectResult>(response);
            Assert.True(result.StatusCode == (int)HttpStatusCode.OK);

            var deviceEventsInformation = Assert.IsType<List<DeviceEventDto>>(result.Value);
            Assert.True(deviceEventsInformation.Count() == numberOfEvents);
        }

        [Fact]
        public async void GetOne_Ok()
        {
            // Arrange
            var numberOfEvents = 40;
            var deviceId = Guid.NewGuid();
            var listOfDeviceEvents = DeviceEventsDtoFactory.GetListOfDeviceEvents(deviceId, numberOfEvents);
            var theEvent = listOfDeviceEvents.LastOrDefault();

            var readerServiceMock = new Mock<IDeviceEventReadService>(MockBehavior.Strict);
            var writerServiceMock = new Mock<IDeviceEventWriteService>(MockBehavior.Strict);

            readerServiceMock.Setup(f => f.GetAsync(theEvent.Id)).Returns(Task.FromResult(theEvent));

            var controller = new DeviceEventsController(writerServiceMock.Object, readerServiceMock.Object);

            // Act
            var response = await controller.Get(theEvent.Id);

            // Assert
            var result = Assert.IsType<OkObjectResult>(response);
            Assert.True(result.StatusCode == (int)HttpStatusCode.OK);

            var deviceEvent = Assert.IsType<DeviceEventDto>(result.Value);
            Assert.True(theEvent == deviceEvent);
        }

        [Fact]
        public async void GetOne_NonExistingEvent()
        {
            // Arrange            
            var eventId = Guid.NewGuid();

            var readerServiceMock = new Mock<IDeviceEventReadService>(MockBehavior.Strict);
            var writerServiceMock = new Mock<IDeviceEventWriteService>(MockBehavior.Strict);

            readerServiceMock.Setup(f => f.GetAsync(eventId)).ReturnsAsync(null);

            var controller = new DeviceEventsController(writerServiceMock.Object, readerServiceMock.Object);

            // Act
            var response = await controller.Get(eventId);

            // Assert
            var result = Assert.IsType<NotFoundObjectResult>(response);
            Assert.True(result.StatusCode == (int)HttpStatusCode.NotFound);

            var errorMessage = Assert.IsType<string>(result.Value);
            Assert.Contains(eventId.ToString(), errorMessage);
        }
    }
}
