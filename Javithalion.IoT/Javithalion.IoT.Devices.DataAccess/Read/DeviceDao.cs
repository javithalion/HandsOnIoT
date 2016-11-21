using System;
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

        public async Task<IEnumerable<Device>> FindAllAsync()
        {            
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                await db.OpenAsync();
                return await db.QueryAsync<Device>(@"Select * From Devices where Disabled = 0");
            }
        }

        public async Task<Device> GetAsync(Guid id)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                await db.OpenAsync();
                return await db.QueryFirstOrDefaultAsync<Device>("Select * From Devices where Id = @Id and Disabled = 0", new { id });
            }
        }
    }
}
