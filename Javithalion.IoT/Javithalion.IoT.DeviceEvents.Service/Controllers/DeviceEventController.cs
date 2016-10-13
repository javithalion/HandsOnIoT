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
using Javithalion.IoT.Infraestructure.ModelBus;

namespace Javithalion.IoT.DeviceEvents.Service.Controllers
{
    [Route("api/[controller]")]
    public class DeviceEventController : Controller
    {
        private readonly IDeviceEventReadService _deviceEventReadService;
        private readonly IServiceBus _serviceBus;

        public DeviceEventController(IDeviceEventReadService deviceEventReadService)
        {
            //_serviceBus = serviceBus;
            _deviceEventReadService = deviceEventReadService;
        }

        [HttpGet()]
        //Unfortunately OData is not available at the moment on dot net core, dirty filtering
        public async Task<IActionResult> Get(string deviceId = "", int page = 1, int pageSize = 50)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _deviceEventReadService.FindAllForDeviceAsync(deviceId, page, pageSize);
            return Ok(result);
        }

        [HttpGet("{eventId:string}")]
        public async Task<IActionResult> Get(string eventId)
        {           
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var theEvent = await _deviceEventReadService.GetAsync(eventId);

            if (theEvent == null)
                return NotFound($"Device event with id = {eventId} not found");
            else
                return Ok(theEvent);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateDeviceEventCommand createCommand)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _serviceBus.Send(createCommand);

            DeviceEventDto temp = new DeviceEventDto();
            return Created($"/api/DeviceEvent/{temp.Id}", temp);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(UpdateDeviceEventCommand updateCommand)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _serviceBus.Send(updateCommand);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(DeleteDeviceEventCommand deleteCommand)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _serviceBus.Send(deleteCommand);
            return Ok();
        }
    }
}
