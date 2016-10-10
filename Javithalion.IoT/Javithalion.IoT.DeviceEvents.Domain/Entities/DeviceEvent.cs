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
        public Guid Id { get; set; }

        [BsonElement("DeviceId")]
        public Guid DeviceId { get; set; }

        [BsonElement("Date")]
        public DateTime Date { get; set; }

    }
}
