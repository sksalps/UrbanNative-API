using Dapper;
using System.Data;
using UrbanNative.Application.DTOs.Customers;
using UrbanNative.Application.Interfaces.Customers;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories.Customers
{

    public class CustomerDashboardRepository : ICustomerDashboardRepository
    {
        
        private readonly SqlConnectionFactory _connectionFactory;

        public CustomerDashboardRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<CustomersDashboardDto> GetDashboardAsync(int userId)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@UserID", userId, DbType.Int32);

            using var multi = await connection.QueryMultipleAsync(
                "sp_CustomerDashboard_Get",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            // 🔷 1. Welcome
            var welcome = await multi.ReadFirstOrDefaultAsync<WelcomeDto>();

            // 🔷 2. Shipment Snapshot
            var shipment = await multi.ReadFirstOrDefaultAsync<ShipmentSnapshotDto>();

            // 🔷 3. Recent Orders
            var recentOrders = (await multi.ReadAsync<RecentOrderDto>()).ToList();

            // 🔷 4. Referral
            var referral = await multi.ReadFirstOrDefaultAsync<ReferralDto>();

            // 🔷 5. Shipment Activities
            var shipmentActivities = (await multi.ReadAsync<ShipmentActivityDto>()).ToList();

            return new CustomersDashboardDto
            {
                Welcome = welcome,
                Shipment = shipment,
                RecentOrders = recentOrders,
                Referral = referral,
                ShipmentActivities = shipmentActivities
            };
        }
    }
}
