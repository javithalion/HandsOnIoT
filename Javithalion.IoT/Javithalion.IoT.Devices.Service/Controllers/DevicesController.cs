using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Javithalion.IoT.Devices.Business;
using Javithalion.IoT.Devices.Business.ReadModel;
using Javithalion.IoT.Devices.Business.WriteModel.Commands;

namespace Javithalion.IoT.Devices.Service.Controllers
{
    [Route("api/[controller]")]
    public class DevicesController : Controller
    {
        private readonly IDeviceWriteService _deviceWriteService;
        private readonly IDeviceReadService _deviceReadService;
        private readonly IPredictionsService _predictionService;


        public DevicesController(IDeviceWriteService deviceWriteService, IDeviceReadService deviceReadService, IPredictionsService predictionService)
        {
            _deviceWriteService = deviceWriteService;
            _deviceReadService = deviceReadService;
            _predictionService = predictionService;
        }

        [HttpGet()]        
        public async Task<IActionResult> Get([FromQuery]string searchText)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _deviceReadService.FindAllAsync(searchText);
            return Ok(result);
        }
       
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var device = await _deviceReadService.GetAsync(id);

            if (device == null)
                return NotFound($"Device with id = {id} not found");
            else
                return Ok(device);
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateDeviceCommand createCommand)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newEvent = await _deviceWriteService.CreateAsync(createCommand);

            return Created($"/api/Devices/{newEvent.Id}", newEvent);
        }
        
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]UpdateDeviceCommand updateCommand)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _deviceWriteService.UpdateAsync(updateCommand);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Device with id = {updateCommand.Id} not found");
            }
        }

        [HttpDelete("{deviceId}")]
        public async Task<IActionResult> Delete(Guid deviceId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var deleteCommand = new DeleteDeviceCommand() { Id = deviceId };

                await _deviceWriteService.DeleteAsync(deleteCommand);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Device event with id = {deviceId} not found");
            }
        }

        [HttpGet("SwitchedOnForecast/{date?}")]
        public async Task<IActionResult> SwitchedOnHourlyPrediction(DateTime? date)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var requestedDate = date ?? DateTime.Now;
            var result = await _predictionService.HourlySwitchedOnAsync(requestedDate);

            return Ok(result);
        }
    }
}
