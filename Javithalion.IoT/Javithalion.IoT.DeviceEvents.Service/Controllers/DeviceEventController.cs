﻿using System;
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
using Javithalion.IoT.DeviceEvents.Business;

namespace Javithalion.IoT.DeviceEvents.Service.Controllers
{
    [Route("api/[controller]")]
    public class DeviceEventController : Controller
    {
        private readonly IDeviceEventWriteService _deviceEventWriteService;
        private readonly IDeviceEventReadService _deviceEventReadService;


        public DeviceEventController(IDeviceEventWriteService deviceEventWriteService, IDeviceEventReadService deviceEventReadService)
        {
            _deviceEventWriteService = deviceEventWriteService;
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

        [HttpGet("{eventId}")]
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

            var newEvent = await _deviceEventWriteService.CreateAsync(createCommand);

            return Created($"/api/DeviceEvent/{newEvent.Id}", newEvent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(UpdateDeviceEventCommand updateCommand)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(DeleteDeviceEventCommand deleteCommand)
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
