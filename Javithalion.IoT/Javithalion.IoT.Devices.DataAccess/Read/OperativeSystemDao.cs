using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Javithalion.IoT.Devices.Domain.Entities;
using System.Data.SqlClient;
using Dapper;

namespace Javithalion.IoT.Devices.DataAccess.Read
{
    public class OperativeSystemDao : IOperativeSystemDao
    {
        private readonly string _connectionString;

        public OperativeSystemDao(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<OperativeSystem>> FindAllAsync()
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                await db.OpenAsync();
                return await db.QueryAsync<OperativeSystem>(@"Select * From OperativeSystems");
            }
        }
    }
}
