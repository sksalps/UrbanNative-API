using System.Data;
using Dapper;
using System;
using System.Collections.Generic;
using UrbanNative.Application.DTOs.AdminGST;
using UrbanNative.Application.Interfaces;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    public class AdminGSTRepository : IAdminGSTRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public AdminGSTRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        

        public async Task<IEnumerable<AdminGSTListDto>> GetGSTAsync(
            decimal? gstPercentage,
            bool? isActive)
        {
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryAsync<AdminGSTListDto>(
                "sp_AdminGST_GetAll",
                new { GSTPercentage = gstPercentage, IsActive = isActive },
                commandType: CommandType.StoredProcedure);
        }
        public async Task<bool> ToggleGSTAsync(int gstId, int adminId)
        {
            using var conn = _connectionFactory.CreateConnection();

            var result = await conn.ExecuteScalarAsync<int>(
                "sp_AdminGST_ToggleActive",
                new { GSTID = gstId, AdminID = adminId },
                commandType: CommandType.StoredProcedure);

            return result == 1;
        }

    }
}
