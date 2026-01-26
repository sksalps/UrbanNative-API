using Dapper;
using System.Data;
using UrbanNative.Application.DTOs.Vendors.Inventory;
using UrbanNative.Application.Interfaces;
using UrbanNative.Application.Interfaces.Vendors;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    public class VendorInventoryRepository : IVendorInventoryRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public VendorInventoryRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // =========================================
        // Inventory Summary (Single Row)
        // =========================================
        public async Task<VendorInventorySummaryDto> GetInventorySummaryAsync(
            int skuId,
            int addressId,
            DateTime fromDate,
            DateTime toDate)
        {
            using var conn = _connectionFactory.CreateConnection();

            var param = new DynamicParameters();
            param.Add("@SKUId", skuId, DbType.Int32);
            param.Add("@AddressID", addressId, DbType.Int32);
            param.Add("@FromDate", fromDate.Date, DbType.Date);
            param.Add("@ToDate", toDate.Date, DbType.Date);

            return await conn.QueryFirstOrDefaultAsync<VendorInventorySummaryDto>(
                "sp_VendorInventory_Summary",
                param,
                commandType: CommandType.StoredProcedure
            ) ?? new VendorInventorySummaryDto();
        }

        // =========================================
        // Inventory Logs (Paged List)
        // =========================================
        public async Task<IReadOnlyList<VendorInventoryLogDto>> GetInventoryLogsAsync(
            int skuId,
            int addressId,
            DateTime fromDate,
            DateTime toDate,
            int page,
            int pageSize)
        {
            using var conn = _connectionFactory.CreateConnection();

            var param = new DynamicParameters();
            param.Add("@SKUId", skuId, DbType.Int32);
            param.Add("@AddressID", addressId, DbType.Int32);
            param.Add("@FromDate", fromDate.Date, DbType.Date);
            param.Add("@ToDate", toDate.Date, DbType.Date);
            param.Add("@Page", page, DbType.Int32);
            param.Add("@PageSize", pageSize, DbType.Int32);

            var data = await conn.QueryAsync<VendorInventoryLogDto>(
                "sp_VendorInventory_Logs",
                param,
                commandType: CommandType.StoredProcedure
            );

            return data.AsList();
        }
    }
}
