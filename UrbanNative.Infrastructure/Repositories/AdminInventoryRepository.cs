using Dapper;
using System.Data;
using UrbanNative.Application.DTOs.AdminInventory;
using UrbanNative.Application.GlobalCall.VariantValueSignature;
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

        // OLD VERSION WITHOUT VARIANT DISPLAY
       /* public async Task<IEnumerable<AdminInventorySkuDto>> GetInventorySkusAsync(int productId)
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
        }*/

        // NEW VERSION WITH VARIANT DISPLAY
        public async Task<IEnumerable<AdminInventorySkuDto>> GetInventorySkusAsync(int productId)
        {
            using var conn = _connectionFactory.CreateConnection();

            // 🔹 SAME AS SKU ENGINE
            var variantNames = await GetVariantNamesAsync(conn);
            var valueNames = await GetVariantValueNamesAsync(conn);

            var list = (await conn.QueryAsync<AdminInventorySkuDto>(
                "sp_AdminInventory_GetSKUs",
                new { ProductId = productId },
                commandType: CommandType.StoredProcedure
            )).ToList();

            foreach (var sku in list)
            {
                sku.VariantDisplay =
                    ValueSignatureDecoder.Decode(sku.ValueSignature,   variantNames,    valueNames      );
            }

            return list;
        }


        private async Task<Dictionary<int, string>> GetVariantNamesAsync(IDbConnection conn)
        {
            var sql = "SELECT VariantID, VariantName FROM VariantMaster WHERE IsActive = 1";
            var rows = await conn.QueryAsync<(int VariantID, string VariantName)>(sql);
            return rows.ToDictionary(x => x.VariantID, x => x.VariantName);
        }

        private async Task<Dictionary<int, string>> GetVariantValueNamesAsync(IDbConnection conn)
        {
            var sql = "SELECT VariantValueID, ValueName FROM VariantValues WHERE IsActive = 1";
            var rows = await conn.QueryAsync<(int VariantValueID, string ValueName)>(sql);
            return rows.ToDictionary(x => x.VariantValueID, x => x.ValueName);
        }

    }
}


