using Javithalion.IoT.DeviceEvents.Domain.Entities;
using System;
using Xunit;

namespace Javithalion.IoT.DeviceEvents.Domain.Tests.EntitiesTestFixture
{
    public class DeviceEventTestFixture
    {
        [Fact]
        public void CreateNewStartUpEvent_Ok()
        {
            var deviceId = Guid.NewGuid();

            var startUpEvent = DeviceEvent.NewStartUpEvent(deviceId);

            Assert.NotNull(startUpEvent);
            Assert.True(startUpEvent.DeviceId == deviceId);
            Assert.True(startUpEvent.Type == EventType.StartUp);
            Assert.True(startUpEvent.TypeName == EventType.StartUp.ToString());
            Assert.True(startUpEvent.Details == string.Empty);
            Assert.True(startUpEvent.Date < DateTime.Now);
            Assert.False(startUpEvent.Deleted);
        }

        [Fact]
        public void CreateNewTearDownEvent_Ok()
        {
            var deviceId = Guid.NewGuid();

            var startUpEvent = DeviceEvent.NewTearDownEvent(deviceId);

            Assert.NotNull(startUpEvent);
            Assert.True(startUpEvent.DeviceId == deviceId);
            Assert.True(startUpEvent.Type == EventType.TearDown);
            Assert.True(startUpEvent.TypeName == EventType.TearDown.ToString());
            Assert.True(startUpEvent.Details == string.Empty);
            Assert.True(startUpEvent.Date < DateTime.Now);
            Assert.False(startUpEvent.Deleted);
        }

        [Fact]
        public void CreateNewResourcesOverviewEvent_Ok()
        {
            var deviceId = Guid.NewGuid();
            var details = new { Data1 = "XXX", Data2 = "YYY" };

            var startUpEvent = DeviceEvent.NewResourcesOverviewEvent(deviceId, details);

            Assert.NotNull(startUpEvent);
            Assert.True(startUpEvent.DeviceId == deviceId);
            Assert.True(startUpEvent.Type == EventType.ResourcesOverview);
            Assert.True(startUpEvent.TypeName == EventType.ResourcesOverview.ToString());
            Assert.True(startUpEvent.Details.ToString() == details.ToString());
            Assert.True(startUpEvent.Date < DateTime.Now);
            Assert.False(startUpEvent.Deleted);
        }

        [Fact]
        public void CreateNewResourcesDetailsEvent_Ok()
        {
            var deviceId = Guid.NewGuid();
            var details = new { Data1 = "XXX", Data2 = "YYY" };

            var startUpEvent = DeviceEvent.NewResourcesDetailedEvent(deviceId, details);

            Assert.NotNull(startUpEvent);
            Assert.True(startUpEvent.DeviceId == deviceId);
            Assert.True(startUpEvent.Type == EventType.ResourcesDetailed);
            Assert.True(startUpEvent.TypeName == EventType.ResourcesDetailed.ToString());
            Assert.True(startUpEvent.Details.ToString() == details.ToString());
            Assert.True(startUpEvent.Date < DateTime.Now);
            Assert.False(startUpEvent.Deleted);
        }

        [Fact]
        public void CreateNewCustomEvent_Ok()
        {
            var deviceId = Guid.NewGuid();
            var customType = "MyBrandNewType";
            var details = new { Data1 = "XXX", Data2 = "YYY" };

            var startUpEvent = DeviceEvent.NewCustomEvent(deviceId, customType, details);

            Assert.NotNull(startUpEvent);
            Assert.True(startUpEvent.DeviceId == deviceId);
            Assert.True(startUpEvent.Type == EventType.Others);
            Assert.True(startUpEvent.TypeName == customType);
            Assert.True(startUpEvent.Details.ToString() == details.ToString());
            Assert.True(startUpEvent.Date < DateTime.Now);
            Assert.False(startUpEvent.Deleted);
        }

        [Fact]
        public void CreateNewCustomEventWithoutDetails_Ok()
        {
            var deviceId = Guid.NewGuid();
            var customType = "MyBrandNewType";

            var startUpEvent = DeviceEvent.NewCustomEvent(deviceId, customType);

            Assert.NotNull(startUpEvent);
            Assert.True(startUpEvent.DeviceId == deviceId);
            Assert.True(startUpEvent.Type == EventType.Others);
            Assert.True(startUpEvent.TypeName == customType);
            Assert.True(startUpEvent.Details.ToString() == string.Empty);
            Assert.True(startUpEvent.Date < DateTime.Now);
            Assert.False(startUpEvent.Deleted);
        }

        [Fact]
        public void CreateNewCustomEventWithEmptyType_Ok()
        {
            var deviceId = Guid.NewGuid();
            var customType = string.Empty;

            Exception ex = Assert.Throws<ArgumentException>(() => DeviceEvent.NewCustomEvent(deviceId, customType, null));
            Assert.Contains("Provided type cannot be null or empty on a custom event", ex.Message);
        }

        [Fact]
        public void CreateNewCustomEventWithNullType_Ok()
        {
            var deviceId = Guid.NewGuid();
            string customType = null;

            Exception ex = Assert.Throws<ArgumentException>(() => DeviceEvent.NewCustomEvent(deviceId, customType, null));
            Assert.Contains("Provided type cannot be null or empty on a custom event", ex.Message);
        }

        [Fact]
        public void CanDisableEvent_Ok()
        {
            var deviceId = Guid.NewGuid();

            var startUpEvent = DeviceEvent.NewStartUpEvent(deviceId);
            startUpEvent.Disable();

            Assert.NotNull(startUpEvent);
            Assert.True(startUpEvent.DeviceId == deviceId);
            Assert.True(startUpEvent.Type == EventType.StartUp);
            Assert.True(startUpEvent.TypeName == EventType.StartUp.ToString());
            Assert.True(startUpEvent.Details == string.Empty);
            Assert.True(startUpEvent.Date < DateTime.Now);
            Assert.True(startUpEvent.Deleted);
        }

        [Fact]
        public void CanStablishDetails_Ok()
        {
            var deviceId = Guid.NewGuid();
            var details = new { Data1 = "XXX", Data2 = "YYY" };

            var startUpEvent = DeviceEvent.NewStartUpEvent(deviceId);
            startUpEvent.WithDetails(details);

            Assert.NotNull(startUpEvent);
            Assert.True(startUpEvent.DeviceId == deviceId);
            Assert.True(startUpEvent.Type == EventType.StartUp);
            Assert.True(startUpEvent.TypeName == EventType.StartUp.ToString());
            Assert.True(startUpEvent.Details.ToString() == details.ToString());
            Assert.True(startUpEvent.Date < DateTime.Now);
            Assert.False(startUpEvent.Deleted);
        }

        [Fact]
        public void CanStablishNullDetails_Ok()
        {
            var deviceId = Guid.NewGuid();            

            var startUpEvent = DeviceEvent.NewStartUpEvent(deviceId);
            startUpEvent.WithDetails(null);

            Assert.NotNull(startUpEvent);
            Assert.True(startUpEvent.DeviceId == deviceId);
            Assert.True(startUpEvent.Type == EventType.StartUp);
            Assert.True(startUpEvent.TypeName == EventType.StartUp.ToString());
            Assert.True(startUpEvent.Details.ToString() == string.Empty);
            Assert.True(startUpEvent.Date < DateTime.Now);
            Assert.False(startUpEvent.Deleted);
        }
    }
}
