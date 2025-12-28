using System.Data;
using Dapper;
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

            var result = await conn.QueryAsync<ProductSkuDetailDto>(
                "SP_AdminSKU_GetProductSkus",
                new { ProductID = productId },
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
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
        public async Task<List<VendorSkuCoverageDetailDto>> GetVendorCoverageDetailAsync(
    int productId,
    int vendorId)
        {
            using var conn = _connectionFactory.CreateConnection();

            var result = await conn.QueryAsync<VendorSkuCoverageDetailDto>(
                "SP_AdminSKU_GetVendorCoverageDetail",
                new
                {
                    ProductID = productId,
                    VendorID = vendorId
                },
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

    }
}
