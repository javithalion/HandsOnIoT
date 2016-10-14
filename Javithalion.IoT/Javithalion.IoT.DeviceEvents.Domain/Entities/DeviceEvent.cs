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
        public string Type { get; private set; }

        [BsonElement("Deleted")]
        public bool Deleted { get; private set; }

        private DeviceEvent()
        {
        }

        public static DeviceEvent CreateNewForDevice(Guid deviceId)
        {
            return new DeviceEvent()
            {
                DeviceId = deviceId,
                Date = DateTime.Now,
                Deleted = false,
                Type = "Default"
            };
        }

        public DeviceEvent OfType(string eventType)
        {
            if (string.IsNullOrEmpty(eventType))
                throw new ArgumentException("Event type was null or empty. Please provide a value here", nameof(eventType));

            Type = Type;

            return this;
        }

        public DeviceEvent Disable()
        {
            Deleted = true;
            return this;
        }

    }
}
