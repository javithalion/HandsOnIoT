using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Javithalion.IoT.Devices.Business.ReadModel.DTOs;
using Javithalion.IoT.Devices.Business.WriteModel.Commands;
using AutoMapper;
using Javithalion.IoT.Devices.DataAccess.Write;
using Javithalion.IoT.Devices.Domain.Entities;
using Javithalion.IoT.Devices.DataAccess.Write.Extensions;



namespace Javithalion.IoT.Devices.Business
{
    public class DeviceWriteService : IDeviceWriteService
    {

        private readonly DevicesContext _context;
        private readonly IMapper _mapper;

        public DeviceWriteService(DevicesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DeviceDto> CreateAsync(CreateDeviceCommand createCommand)
        {
            var operativeSystem = await GetOperativeSystemFromCreateCommand(createCommand);
            var device = Device.CreateNew(createCommand.Name, operativeSystem).WithIpAddress(createCommand.IpAddress);

            _context.Devices.Add(device);
            await _context.SaveChangesAsync();

            return _mapper.Map<Device, DeviceDto>(device);
        }

        private async Task<OperativeSystem> GetOperativeSystemFromCreateCommand(CreateDeviceCommand createCommand)
        {
            var operativeSystem = await _context.OperativeSystems.WithIdAsync(createCommand.SelectedOperativeSystemId);
            if (operativeSystem == null)
                throw new KeyNotFoundException($"There is no Operative System identified with the provided value '{createCommand.SelectedOperativeSystemId}'");

            return operativeSystem;
        }

        public async Task<DeviceDto> DeleteAsync(DeleteDeviceCommand deleteCommand)
        {
            var device = await _context.Devices.WithIdAsync(deleteCommand.Id);
            if (device == null)
                throw new KeyNotFoundException();
            else
            {
                device.Disable();

                await _context.SaveChangesAsync();
                return _mapper.Map<Device, DeviceDto>(device);
            }
        }

        public async Task<DeviceDto> UpdateAsync(UpdateDeviceCommand updateCommand)
        {
            var device = await _context.Devices.WithIdAsync(updateCommand.Id);
            if (device == null)
                throw new KeyNotFoundException();
            else
            {
                device.WithName(updateCommand.Name);

                await _context.SaveChangesAsync();
                return _mapper.Map<Device, DeviceDto>(device);
            }
        }
    }
}
