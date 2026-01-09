using Dapper;
using System.Data;
using UrbanNative.Application.DTOs.AdminReturnsOrder;
using UrbanNative.Application.Interfaces;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    public class AdminReturnsRepository : IAdminReturnsRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public AdminReturnsRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<AdminReturnListDto>> GetReturnsAsync(
            string? status,
            int? vendorId,
            DateTime? fromDate,
            DateTime? toDate)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryAsync<AdminReturnListDto>(
                "sp_AdminReturns_GetAll",
                new
                {
                    Status = status,
                    VendorId = vendorId,
                    FromDate = fromDate,
                    ToDate = toDate
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<AdminReturnDetailsDto?> GetReturnDetailsAsync(int returnId)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<AdminReturnDetailsDto>(
                "sp_AdminReturns_GetDetails",
                new { ReturnId = returnId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> ApproveRejectAsync(
            int returnId,
            int adminId,
            bool isApproved,
            string adminComment,
            string remarkText)
        {
            using var conn = _connectionFactory.CreateConnection();

            var rows = await conn.ExecuteAsync(
                "sp_AdminReturns_ApproveReject",
                new
                {
                    ReturnId = returnId,
                    AdminId = adminId,
                    IsApproved = isApproved,
                    AdminComment = adminComment,
                    RemarkText = remarkText
                },
                commandType: CommandType.StoredProcedure);

            return rows > 0;
        }

        public async Task<bool> CreateReturnShipmentAsync(
            int returnId,
            string courierName,
            string trackingNumber,
            string pickupAddress,
            string deliveryAddress)
        {
            using var conn = _connectionFactory.CreateConnection();

            var rows = await conn.ExecuteAsync(
                "sp_AdminReturnShipment_Create",
                new
                {
                    ReturnId = returnId,
                    CourierName = courierName,
                    TrackingNumber = trackingNumber,
                    PickupAddress = pickupAddress,
                    DeliveryAddress = deliveryAddress
                },
                commandType: CommandType.StoredProcedure);

            return rows > 0;
        }
    }
}
