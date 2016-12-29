using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Javithalion.IoT.DeviceEvents.Business.ReadModel;
using Javithalion.IoT.DeviceEvents.Business.WriteModel.Commands;
using Javithalion.IoT.DeviceEvents.Business;

namespace Javithalion.IoT.DeviceEvents.Service.Controllers
{
    [Route("api/[controller]")]
    public class DeviceEventsController : Controller
    {
        private readonly IDeviceEventWriteService _deviceEventWriteService;
        private readonly IDeviceEventReadService _deviceEventReadService;


        public DeviceEventsController(IDeviceEventWriteService deviceEventWriteService, IDeviceEventReadService deviceEventReadService)
        {
            _deviceEventWriteService = deviceEventWriteService;
            _deviceEventReadService = deviceEventReadService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllForDevice(Guid deviceId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _deviceEventReadService.FindAllForDeviceAsync(deviceId);
            return Ok(result);
        }

        [HttpGet("{eventId}")]
        public async Task<IActionResult> Get(Guid eventId)
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
        public async Task<IActionResult> Post([FromBody]CreateDeviceEventCommand createCommand)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newEvent = await _deviceEventWriteService.CreateAsync(createCommand);

            return Created($"/api/DeviceEvents/{newEvent.Id}", newEvent);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]UpdateDeviceEventCommand updateCommand)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _deviceEventWriteService.UpdateAsync(updateCommand);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Device event with id = {updateCommand.EventId} not found");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody]DeleteDeviceEventCommand deleteCommand)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _deviceEventWriteService.DeleteAsync(deleteCommand);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Device event with id = {deleteCommand.EventId} not found");
            }
        }
    }
}
