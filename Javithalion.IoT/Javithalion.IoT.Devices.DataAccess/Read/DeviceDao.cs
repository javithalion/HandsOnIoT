﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Javithalion.IoT.Devices.Domain.Entities;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace Javithalion.IoT.Devices.DataAccess.Read
{
    public class DeviceDao : IDeviceDao
    {
        private readonly string _connectionString;

        public DeviceDao(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Device>> FindAllAsync(string searchText)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                await db.OpenAsync();
                return await db.QueryAsync<Device, OperativeSystem, Device>(@"Select * From Devices dvc
                                                        left join OperativeSystems os on dvc.OperativeSystemId = os.Id    
                                                        where dvc.Disabled = 0
                                                        and dvc.name LIKE @searchText",
                                                        (device, operativeSystem) =>
                                                        {
                                                            device.Running(operativeSystem);
                                                            return device;
                                                        },
                                                        param: new { searchText = $"%{searchText}%" },
                                                        splitOn: "OperativeSystemId");
            }
        }

        public async Task<Device> GetAsync(Guid id)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                await db.OpenAsync();
                //return await db.QueryFirstOrDefaultAsync<Device>(@"Select * From Devices 
                //                                                    left join OperativeSystems os on dvc.OperativeSystemId = os.Id 
                //                                                    where Id = @Id 
                //                                                    and Disabled = 0",
                //                                                    new { id });

                var deviceQuery = await db.QueryAsync<Device, OperativeSystem, Device>(@"Select * From Devices dvc
                                                        left join OperativeSystems os on dvc.OperativeSystemId = os.Id    
                                                        where dvc.Disabled = 0
                                                        and dvc.Id = @Id",
                                                        (device, operativeSystem) =>
                                                        {
                                                            device.Running(operativeSystem);
                                                            return device;
                                                        },
                                                        param: new { id },
                                                        splitOn: "OperativeSystemId");
                return deviceQuery.FirstOrDefault();
            }
        }
    }
}
