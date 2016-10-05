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

        [BsonElement("Date")]
        public DateTime Date { get; set; }


        //protected DeviceEvent()
        //{
        //    Id = Guid.NewGuid();
        //}

        //public DeviceEvent(DateTime date) :
        //    this()
        //{
        //    Date = date;
        //}
    }
}
