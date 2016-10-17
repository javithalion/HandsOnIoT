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

        public async Task<IEnumerable<Device>> FindAllAsync(string searchExpression, int page, int pageSize)
        {
            var offset = (page - 1) * pageSize;
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryAsync<Device>(@"Select * From Devices 
                                                     where Name Like '%@searchExpression%' 
                                                     and Disabled = 0
                                                     order by id 
                                                     OFFSET @offset ROWS 
                                                     FETCH NEXT @pageSize ROWS ONLY", 
                                                     new { searchExpression, offset, pageSize });
            }
        }

        public async Task<Device> GetAsync(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryFirstOrDefaultAsync<Device>("Select * From Devices where Id = @Id and Disabled = 0", new { id });
            }
        }
    }
}
