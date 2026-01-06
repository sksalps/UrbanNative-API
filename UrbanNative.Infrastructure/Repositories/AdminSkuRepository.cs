using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using UrbanNative.Application.DTOs.AdminSKU;
using UrbanNative.Application.Interfaces;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repository
{
    public class AdminSkuRepository : IAdminSkuRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public AdminSkuRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // =====================================================
        // 1️⃣ GENERATE + SAVE SKUs (Admin Action)
        // =====================================================
        public async Task GenerateAndSaveSkusAsync(
            int productId,
            List<VariantSelectionDto> selections,
            decimal price,
            int stock,
            int? returnPolicyId)
        {
            // Generate combinations in C#
            var skuSignatures = SkuCombinationGenerator.Generate(selections);

            if (!skuSignatures.Any())
                throw new Exception("No SKU combinations generated.");

            // Prepare TVP
            var table = new DataTable();
            table.Columns.Add("SeqNo", typeof(int));
            table.Columns.Add("ValueSignature", typeof(string));

            foreach (var sku in skuSignatures)
                table.Rows.Add(sku.SeqNo, sku.ValueSignature);

            using var conn = _connectionFactory.CreateConnection();

            var param = new DynamicParameters();
            param.Add("@ProductID", productId);
            param.Add("@SKUs", table.AsTableValuedParameter("TVP_ProductSKUSignature"));
            param.Add("@Price", price);
            param.Add("@Stock", stock);
            param.Add("@ReturnPolicyID", returnPolicyId);

            await conn.ExecuteAsync(
                "SP_ProductSKU_Insert",
                param,
                commandType: CommandType.StoredProcedure
            );
        }

        // =====================================================
        // 2️⃣ ADMIN SKU OVERVIEW (READ-ONLY)
        // =====================================================
        public async Task<List<AdminSkuOverviewDto>> GetSkuOverviewAsync()
        {
            using var conn = _connectionFactory.CreateConnection();

            var result = await conn.QueryAsync<AdminSkuOverviewDto>(
                "SP_AdminSKU_GetOverview",
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }


        public async Task<List<ProductSkuDetailDto>> GetProductSkusAsync(int productId)
        {
            using var conn = _connectionFactory.CreateConnection();

            var variantNames = await GetVariantNamesAsync(conn);
            var valueNames = await GetVariantValueNamesAsync(conn);

            var result = (await conn.QueryAsync<ProductSkuDetailDto>(
                "SP_AdminSKU_GetProductSkus",
                new { ProductID = productId },
                commandType: CommandType.StoredProcedure
            )).ToList();

            foreach (var sku in result)
            {
                sku.VariantDisplay =
                    DecodeSignature(
                        sku.ValueSignature,
                        variantNames,
                        valueNames
                    );
            }

            return result;
        }


        public async Task<List<AdminVendorSkuCoverageDto>> GetVendorCoverageAsync(int productId,bool includeInactiveVendors)
        {
            using var conn = _connectionFactory.CreateConnection();

            var result = await conn.QueryAsync<AdminVendorSkuCoverageDto>(
                "SP_AdminSKU_GetVendorCoverage",
                new
                {
                    ProductID = productId,
                    IncludeInactiveVendors = includeInactiveVendors
                },
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }
        public async Task<ProductSkuCoverageHeaderDto> GetCoverageHeaderAsync(int productId)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QuerySingleOrDefaultAsync<ProductSkuCoverageHeaderDto>(
                "SP_AdminSKU_GetCoverageHeader",
                new { ProductID = productId },
                commandType: CommandType.StoredProcedure
            );
        }

        // =========================Variant & Value of a SKU============================
        public async Task<List<VendorSkuCoverageDetailDto>> GetVendorCoverageDetailAsync(int productId, int vendorId)
        {
            using var conn = _connectionFactory.CreateConnection();

            var variantNames = await GetVariantNamesAsync(conn);
            var valueNames = await GetVariantValueNamesAsync(conn);

            var raw = await conn.QueryAsync<VendorSkuCoverageDetailDto>(
                "[SP_AdminSKU_GetVendorCoverageDetail]",
                new { ProductID = productId, VendorID = vendorId },
                commandType: CommandType.StoredProcedure
            );

            var list = raw.ToList();

            foreach (var row in list)
            {
                row.DisplayText = DecodeSignature( row.ValueSignature,   variantNames,  valueNames         );
            }

            return list; // 👈 List<T>, not IEnumerable<T>
        }


        private static string DecodeSignature( string signature,Dictionary<int, string> variantNames,
            Dictionary<int, string> valueNames)
        {
            if (string.IsNullOrWhiteSpace(signature))
                return string.Empty;

            var parts = signature.Split('|', StringSplitOptions.RemoveEmptyEntries);
            var readable = new List<string>();

            foreach (var part in parts)
            {
                var ids = part.Split(':');
                if (ids.Length != 2) continue;

                if (!int.TryParse(ids[0], out var variantId)) continue;
                if (!int.TryParse(ids[1], out var valueId)) continue;

                if (!variantNames.TryGetValue(variantId, out var variantName)) continue;
                if (!valueNames.TryGetValue(valueId, out var valueName)) continue;

                readable.Add($"{variantName}: {valueName}");
            }

            return string.Join(", ", readable);
        }

        private async Task<Dictionary<int, string>> GetVariantNamesAsync(IDbConnection conn)
        {
            var sql = @"SELECT VariantID, VariantName FROM VariantMaster WHERE IsActive = 1";
            var rows = await conn.QueryAsync<(int VariantID, string VariantName)>(sql);
            return rows.ToDictionary(x => x.VariantID, x => x.VariantName);
        }

        private async Task<Dictionary<int, string>> GetVariantValueNamesAsync(IDbConnection conn)
        {
            var sql = @"SELECT VariantValueID, ValueName FROM VariantValues WHERE IsActive = 1";
            var rows = await conn.QueryAsync<(int VariantValueID, string ValueName)>(sql);
            return rows.ToDictionary(x => x.VariantValueID, x => x.ValueName);
        }
        // =========================Variant Value End============================

    }
}
