using System.Data;
using Dapper;
using UrbanNative.Application.DTOs.AdminInventory;
using UrbanNative.Application.Interfaces;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    public class AdminInventoryRepository : IAdminInventoryRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public AdminInventoryRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // ============================================
        // Inventory List (Admin Inventory Main Page)
        // ============================================
        public async Task<IEnumerable<AdminInventoryListDto>> GetInventoryAsync(
            string? search,
            int? categoryId,
            bool? lowStockOnly,
            bool? isActive)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryAsync<AdminInventoryListDto>(
                "sp_AdminInventory_GetAll",
                new
                {
                    Search = search,
                    CategoryId = categoryId,
                    LowStockOnly = lowStockOnly,
                    IsActive = isActive
                },
                commandType: CommandType.StoredProcedure
            );
        }

        // ============================================
        // Inventory Product Summary (Header)
        // ============================================
        public async Task<AdminInventorySummaryDto?> GetInventorySummaryAsync(int productId)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<AdminInventorySummaryDto>(
                "sp_AdminInventory_GetSummary",
                new
                {
                    ProductId = productId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        // ============================================
        // Inventory SKU Drill-Down (Read-Only)
        // ============================================
        public async Task<IEnumerable<AdminInventorySkuDto>> GetInventorySkusAsync(int productId)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryAsync<AdminInventorySkuDto>(
                "sp_AdminInventory_GetSKUs",
                new
                {
                    ProductId = productId
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
