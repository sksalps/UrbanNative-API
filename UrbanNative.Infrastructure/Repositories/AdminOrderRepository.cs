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


        public async Task<AdminOrderPagedResultDto> GetOrdersAsync(
            string orderNo,
            DateTime? fromDate,
            DateTime? toDate,
            string paymentStatus,
            string orderStatus,
            int? userId,
            int? vendorId,
            int pageNumber,
            int pageSize)
        {
            using var conn = _connectionFactory.CreateConnection();

            using var multi = await conn.QueryMultipleAsync(
                "sp_AdminOrders_GetAll",
                new
                {
                    OrderNo = orderNo,
                    FromDate = fromDate,
                    ToDate = toDate,
                    PaymentStatus = paymentStatus,
                    OrderStatus = orderStatus,
                    UserID = userId,
                    VendorID = vendorId,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                commandType: CommandType.StoredProcedure
            );

            var orders = (await multi.ReadAsync<AdminOrderListDto>()).ToList();
            var total = await multi.ReadSingleAsync<int>();

            return new AdminOrderPagedResultDto
            {
                Orders = orders,
                TotalRecords = total
            };
        }

        public async Task<AdminOrderDetailsDto> GetOrderDetailsAsync(int orderId)
        {
            using var conn = _connectionFactory.CreateConnection();

            using var multi = await conn.QueryMultipleAsync(
                "sp_AdminOrder_GetDetails",
                new { OrderID = orderId },
                commandType: CommandType.StoredProcedure
            );

            var header = await multi.ReadSingleAsync<AdminOrderDetailsHeaderDto>();
            var items = (await multi.ReadAsync<AdminOrderDetailsItemDto>()).ToList();
            var shipments = (await multi.ReadAsync<AdminOrderDetailsShipmentDto>()).ToList();

            return new AdminOrderDetailsDto
            {
                Header = header,
                Items = items,
                Shipments = shipments
            };
        }
        //*******Order Shipment Related operations can be added here in future****
        public async Task<AdminOrderShipmentDetailsDto> GetOrderShipmentDetailsAsync(int orderId)
        {
            using var conn = _connectionFactory.CreateConnection();

            using var multi = await conn.QueryMultipleAsync(
                "sp_AdminOrderShipments_GetDetails",
                new { OrderID = orderId },
                commandType: CommandType.StoredProcedure
            );

            var header = await multi.ReadSingleAsync<AdminOrderShipmentHeaderDto>();
            var shipments = (await multi.ReadAsync<AdminOrderShipmentListDto>()).ToList();

            return new AdminOrderShipmentDetailsDto
            {
                Header = header,
                Shipments = shipments
            };
        }



        //**********************************************************************//
    }
}
