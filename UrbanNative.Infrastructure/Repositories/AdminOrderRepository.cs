using System.Data;
using Dapper;
using UrbanNative.Application.DTOs.AdminOrders;
using UrbanNative.Application.Interfaces;   // ✅ IMPORTANT
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    public class AdminOrderRepository : IAdminOrderRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public AdminOrderRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<AdminOrderListDto>> GetOrdersAsync(
            DateTime? fromDate,
            DateTime? toDate,
            string orderStatus)
        {
            using var conn = _connectionFactory.CreateConnection();

            var result = await conn.QueryAsync<AdminOrderListDto>(
                "sp_AdminOrders_GetAll",
                new { FromDate = fromDate, ToDate = toDate, OrderStatus = orderStatus },
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        public async Task<AdminOrderDetailsDto> GetOrderDetailsAsync(int orderId)
        {
            using var conn = _connectionFactory.CreateConnection();

            using var multi = await conn.QueryMultipleAsync(
                "sp_AdminOrder_GetById",
                new { OrderID = orderId },
                commandType: CommandType.StoredProcedure
            );

            var order = await multi.ReadSingleAsync<AdminOrderDetailsDto>();
            order.Items = (await multi.ReadAsync<AdminOrderItemDto>()).ToList();
            order.Shipments = (await multi.ReadAsync<AdminOrderShipmentDto>()).ToList();

            return order;
        }
    }
}
