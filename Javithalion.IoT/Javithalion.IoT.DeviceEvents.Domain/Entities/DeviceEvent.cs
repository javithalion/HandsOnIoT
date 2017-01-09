using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javithalion.IoT.DeviceEvents.Domain.Entities
{
    public class DeviceEvent
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid Id { get; private set; }

        [BsonElement("DeviceId")]
        public Guid DeviceId { get; private set; }

        [BsonElement("Date")]
        public DateTime Date { get; private set; }

        [BsonElement("Type")]
        public EventType Type { get; private set; }

        [BsonElement("TypeName")]
        public string TypeName { get; private set; }

        [BsonElement("EventDetails")]
        public dynamic Details { get; private set; }

        [BsonElement("Deleted")]
        public bool Deleted { get; private set; }

        private DeviceEvent(Guid deviceId)
        {
            if (deviceId == Guid.Empty)
                throw new ArgumentException($"Device event should be related with a valid device. Provided Device ID was {deviceId}", nameof(deviceId));

            DeviceId = deviceId;
            Date = DateTime.Now;
            Deleted = false;
        }

        public static DeviceEvent NewStartUpEvent(Guid deviceId)
        {
            return new DeviceEvent(deviceId)
            {
                Type = EventType.StartUp,
                TypeName = EventType.StartUp.ToString(),
                Details = string.Empty
            };
        }

        public static DeviceEvent NewTearDownEvent(Guid deviceId)
        {
            return new DeviceEvent(deviceId)
            {                
                Type = EventType.TearDown,
                TypeName = EventType.TearDown.ToString(),
                Details = string.Empty
            };
        }

        public static DeviceEvent NewResourcesOverviewEvent(Guid deviceId, dynamic details)
        {
            return new DeviceEvent(deviceId)
            {                
                Type = EventType.ResourcesOverview,
                TypeName = EventType.ResourcesOverview.ToString(),
                Details = details
            };
        }

        public static DeviceEvent NewResourcesDetailedEvent(Guid deviceId, dynamic details)
        {
            return new DeviceEvent(deviceId)
            {
                Type = EventType.ResourcesDetailed,
                TypeName = EventType.ResourcesDetailed.ToString(),
                Details = details
            };
        }

        public static DeviceEvent NewCustomEvent(Guid deviceId, string type, dynamic details = null)
        {
            if (string.IsNullOrEmpty(type))
                throw new ArgumentException("Provided type cannot be null or empty on a custom event", nameof(type));

            return new DeviceEvent(deviceId)
            {               
                Type = EventType.Others,
                TypeName = type,
                Details = details ?? string.Empty
            };
        }

        public DeviceEvent WithDetails(dynamic details)
        {
            Details = details ?? string.Empty;

            return this;
        }

        public DeviceEvent Disable()
        {
            Deleted = true;
            return this;
        }

    }
}
