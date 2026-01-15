using Dapper;
using System.Data;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    public class VendorDashboardRepository : IVendorDashboardRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public VendorDashboardRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<VendorDashboardDto> GetVendorDashboardAsync(int vendorId)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QuerySingleAsync<VendorDashboardDto>(
                "sp_VendorDashboard_GetStats",
                new { VendorId = vendorId },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
