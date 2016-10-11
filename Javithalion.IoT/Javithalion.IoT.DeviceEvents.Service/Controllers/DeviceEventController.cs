using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Javithalion.IoT.DeviceEvents.Business.ReadModel;
using Javithalion.IoT.DeviceEvents.Domain.Entities;
using Javithalion.IoT.DeviceEvents.Business.WriteModel.Commands;
using Javithalion.IoT.DeviceEvents.Business.ReadModel.DTOs;
using System.Net.Http;
using System.Net;

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

        [HttpGet()]
        //Unfortunately OData is not available at the moment on dot net core, dirty filtering
        public async Task<IActionResult> Get(string deviceId = "", int page = 1, int pageSize = 50)
        {
            var result = await _deviceEventReadService.FindAllForDeviceAsync(deviceId);
            return Ok(result);
        }

        [HttpGet("{eventId:string}")]
        public async Task<IActionResult> Get(string eventId)
        {
            var theEvent = await _deviceEventReadService.GetAsync(eventId);

            if (theEvent == null)                            
                return NotFound($"Device event with id = {eventId} not found");            
            else            
                return  Ok(theEvent);            
        }

        [HttpPost]
        public IActionResult Post(CreateDeviceEventCommand createCommand)
        {

            DeviceEventDto temp = new DeviceEventDto();
            return Created($"/api/DeviceEvent/{temp.Id}",temp);
        }

        [HttpPut("{id}")]
        public void Put(UpdateDeviceEventCommand updateCommand)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(DeleteDeviceEventCommand deleteCommand)
        {
        }
    }
}
