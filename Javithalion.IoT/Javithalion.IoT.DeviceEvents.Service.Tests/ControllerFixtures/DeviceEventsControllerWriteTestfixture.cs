using Javithalion.IoT.DeviceEvents.Business;
using Javithalion.IoT.DeviceEvents.Business.ReadModel;
using Javithalion.IoT.DeviceEvents.Business.ReadModel.DTOs;
using Javithalion.IoT.DeviceEvents.Business.WriteModel.Commands;
using Javithalion.IoT.DeviceEvents.Service.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Javithalion.IoT.DeviceEvents.Service.Tests.ControllerFixtures
{
    public class DeviceEventsControllerWriteTestfixture
    {
        [Fact(DisplayName = "CreateOne_InformationOk")]
        public async void CreateOne_InformationOk()
        {
            // Arrange            
            var createCommand = new CreateDeviceEventCommand()
            {
                DeviceId = Guid.NewGuid(),
                Details = "My details",
                EventType = EventType.Others,
                TypeName = "Custom"
            };

            var brandNewEvent = new DeviceEventDto()
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                Details = createCommand.Details,
                DeviceId = createCommand.DeviceId,
                Type = createCommand.TypeName
            };

            var readerServiceMock = new Mock<IDeviceEventReadService>(MockBehavior.Strict);
            var writerServiceMock = new Mock<IDeviceEventWriteService>(MockBehavior.Strict);

            writerServiceMock.Setup(f => f.CreateAsync(createCommand)).ReturnsAsync(brandNewEvent);

            var controller = new DeviceEventsController(writerServiceMock.Object, readerServiceMock.Object);

            // Act
            var response = await controller.Post(createCommand);

            // Assert
            var result = Assert.IsType<CreatedResult>(response);
            Assert.True(result.StatusCode == (int)HttpStatusCode.Created);

            var createdObject = Assert.IsType<DeviceEventDto>(result.Value);
            Assert.True(createdObject.Id == brandNewEvent.Id &&
                        createdObject.Date == brandNewEvent.Date &&
                        createdObject.Details.ToString() == brandNewEvent.Details.ToString() &&
                        createdObject.DeviceId == brandNewEvent.DeviceId &&
                        createdObject.Type == brandNewEvent.Type);
            Assert.EndsWith(createdObject.Id.ToString(), result.Location);
        }

        [Fact(DisplayName = "CreateOne_InformationNOk")]
        public async void CreateOne_InformationNOk()
        {
            // Arrange            
            var createCommand = new CreateDeviceEventCommand()
            {
                DeviceId = Guid.Empty,
                Details = null,
                EventType = EventType.Others,
                TypeName = null
            };

            var readerServiceMock = new Mock<IDeviceEventReadService>(MockBehavior.Strict);
            var writerServiceMock = new Mock<IDeviceEventWriteService>(MockBehavior.Strict);

            var controller = new DeviceEventsController(writerServiceMock.Object, readerServiceMock.Object);

            // Act
            var response = await controller.Post(createCommand);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(response);
            Assert.True(result.StatusCode == (int)HttpStatusCode.BadRequest);
        }

        [Fact(DisplayName = "CreateOne_CreateServiceFailed")]
        public async void CreateOne_CreateServiceFailed()
        {
            // Arrange            
            string errorMessage = "Error when creating";

            var createCommand = new CreateDeviceEventCommand()
            {
                DeviceId = Guid.NewGuid(),
                Details = "My details",
                EventType = EventType.Others,
                TypeName = "Custom"
            };

            var readerServiceMock = new Mock<IDeviceEventReadService>(MockBehavior.Strict);
            var writerServiceMock = new Mock<IDeviceEventWriteService>(MockBehavior.Strict);

            writerServiceMock.Setup(f => f.CreateAsync(createCommand)).ThrowsAsync(new Exception(errorMessage));

            var controller = new DeviceEventsController(writerServiceMock.Object, readerServiceMock.Object);

            // Act
            Exception ex = await Assert.ThrowsAsync<Exception>(() => controller.Post(createCommand));

            // Assert
            Assert.Equal(errorMessage, ex.Message);
        }

        [Fact(DisplayName = "UpdateOne_InformationOk")]
        public async void UpdateOne_InformationOk()
        {
            // Arrange
            var updateCommand = new UpdateDeviceEventCommand()
            {
                EventId = Guid.NewGuid(),
                Details = "My details"
            };

            var updatedEvent = new DeviceEventDto()
            {
                Id = updateCommand.EventId,
                Date = DateTime.Now,
                Details = updateCommand.Details,
                DeviceId = Guid.NewGuid(),
                Type = "Custom"
            };

            var readerServiceMock = new Mock<IDeviceEventReadService>(MockBehavior.Strict);
            var writerServiceMock = new Mock<IDeviceEventWriteService>(MockBehavior.Strict);

            writerServiceMock.Setup(f => f.UpdateAsync(updateCommand)).ReturnsAsync(updatedEvent);

            var controller = new DeviceEventsController(writerServiceMock.Object, readerServiceMock.Object);

            // Act
            var response = await controller.Put(updateCommand);

            // Assert
            var result = Assert.IsType<OkResult>(response);
            Assert.True(result.StatusCode == (int)HttpStatusCode.OK);
        }

        [Fact(DisplayName = "UpdateOne_NonExisitingDeviceEvent")]
        public async void UpdateOne_NonExisitingDeviceEvent()
        {
            // Arrange                
            var updateCommand = new UpdateDeviceEventCommand()
            {
                EventId = Guid.NewGuid(),
                Details = "My details"
            };

            var readerServiceMock = new Mock<IDeviceEventReadService>(MockBehavior.Strict);
            var writerServiceMock = new Mock<IDeviceEventWriteService>(MockBehavior.Strict);

            writerServiceMock.Setup(f => f.UpdateAsync(updateCommand)).ThrowsAsync(new KeyNotFoundException());

            var controller = new DeviceEventsController(writerServiceMock.Object, readerServiceMock.Object);

            // Act
            var response = await controller.Put(updateCommand);

            // Assert
            var result = Assert.IsType<NotFoundObjectResult>(response);
            Assert.True(result.StatusCode == (int)HttpStatusCode.NotFound);
        }

        [Fact(DisplayName = "UpdateOne_UpdateServiceFailed")]
        public async void UpdateOne_UpdateServiceFailed()
        {
            // Arrange     
            var errorMessage = "Error!";
            var updateCommand = new UpdateDeviceEventCommand()
            {
                EventId = Guid.NewGuid(),
                Details = "My details",
            };

            var readerServiceMock = new Mock<IDeviceEventReadService>(MockBehavior.Strict);
            var writerServiceMock = new Mock<IDeviceEventWriteService>(MockBehavior.Strict);

            writerServiceMock.Setup(f => f.UpdateAsync(updateCommand)).ThrowsAsync(new Exception(errorMessage));

            var controller = new DeviceEventsController(writerServiceMock.Object, readerServiceMock.Object);

            // Act
            Exception ex = await Assert.ThrowsAsync<Exception>(() => controller.Put(updateCommand));

            // Assert
            Assert.Equal(errorMessage, ex.Message);
        }

        [Fact(DisplayName = "RemoveOne_InformationOk")]
        public async void RemoveOne_InformationOk()
        {
            // Arrange            
            var deleteCommand = new DeleteDeviceEventCommand()
            {
                EventId = Guid.NewGuid()                
            };

            var deletedEvent = new DeviceEventDto()
            {
                Id = deleteCommand.EventId,
                Date = DateTime.Now,
                Details = "Details",
                DeviceId = Guid.NewGuid(),
                Type = "Custom"
            };

            var readerServiceMock = new Mock<IDeviceEventReadService>(MockBehavior.Strict);
            var writerServiceMock = new Mock<IDeviceEventWriteService>(MockBehavior.Strict);

            writerServiceMock.Setup(f => f.DeleteAsync(deleteCommand)).ReturnsAsync(deletedEvent);

            var controller = new DeviceEventsController(writerServiceMock.Object, readerServiceMock.Object);

            // Act
            var response = await controller.Delete(deleteCommand);

            // Assert
            var result = Assert.IsType<OkResult>(response);
            Assert.True(result.StatusCode == (int)HttpStatusCode.OK);            
        }

        [Fact(DisplayName = "RemoveOne_NonExisitingDeviceEvent")]
        public async void RemoveOne_NonExisitingDeviceEvent()
        {
            // Arrange            
            var deleteCommand = new DeleteDeviceEventCommand()
            {
                EventId = Guid.NewGuid()
            };           

            var readerServiceMock = new Mock<IDeviceEventReadService>(MockBehavior.Strict);
            var writerServiceMock = new Mock<IDeviceEventWriteService>(MockBehavior.Strict);

            writerServiceMock.Setup(f => f.DeleteAsync(deleteCommand)).ThrowsAsync(new KeyNotFoundException());

            var controller = new DeviceEventsController(writerServiceMock.Object, readerServiceMock.Object);

            // Act
            var response = await controller.Delete(deleteCommand);

            // Assert
            var result = Assert.IsType<NotFoundObjectResult>(response);
            Assert.True(result.StatusCode == (int)HttpStatusCode.NotFound);
        }

        [Fact(DisplayName = "RemoveOne_RemoveServiceFailed")]
        public async void RemoveOne_RemoveServiceFailed()
        {
            // Arrange            
            string errorMessage = "Error when deleting";
                        
            var deleteCommand = new DeleteDeviceEventCommand()
            {
                EventId = Guid.NewGuid()
            };

            var readerServiceMock = new Mock<IDeviceEventReadService>(MockBehavior.Strict);
            var writerServiceMock = new Mock<IDeviceEventWriteService>(MockBehavior.Strict);

            writerServiceMock.Setup(f => f.DeleteAsync(deleteCommand)).ThrowsAsync(new Exception(errorMessage));

            var controller = new DeviceEventsController(writerServiceMock.Object, readerServiceMock.Object);

            // Act
            Exception ex = await Assert.ThrowsAsync<Exception>(() => controller.Delete(deleteCommand));

            // Assert
            Assert.Equal(errorMessage, ex.Message);
        }
    }
}
