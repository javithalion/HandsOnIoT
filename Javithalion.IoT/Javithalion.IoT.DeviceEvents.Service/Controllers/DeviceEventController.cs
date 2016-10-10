using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Javithalion.IoT.DeviceEvents.Business.ReadModel;
using Javithalion.IoT.DeviceEvents.Domain.Entities;

namespace Javithalion.IoT.DeviceEvents.Service.Controllers
{
    [Route("api/[controller]")]
    public class DeviceEventController : Controller
    {
        private IDeviceEventReadService _deviceEventReadService;

        public DeviceEventController(IDeviceEventReadService deviceEventReadService)
        {
            _deviceEventReadService = deviceEventReadService;
        }

        // GET api/values
        [HttpGet("{deviceId}")]
        public async Task<IEnumerable<DeviceEvent>> Get(string deviceId)
        {
            return await _deviceEventReadService.FindAllForDeviceAsync(deviceId);
        }

        // GET api/values/5
        [HttpGet("{deviceId}/{eventId}")]
        public async Task<DeviceEvent> Get(string deviceId, string eventId)
        {
            return await _deviceEventReadService.GetAsync(deviceId, eventId);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
