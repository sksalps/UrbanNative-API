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

        public async Task GenerateAndSaveSkusAsync(
            int productId,
            List<VariantSelectionDto> selections,
            decimal price,
            int stock,
            int? returnPolicyId)
        {
            // 1️⃣ Generate SKU combinations (C#)
            var skuSignatures = SkuCombinationGenerator.Generate(selections);

            if (!skuSignatures.Any())
                throw new Exception("No SKU combinations generated.");

            // 2️⃣ Prepare TVP
            var table = new DataTable();
            table.Columns.Add("SeqNo", typeof(int));
            table.Columns.Add("ValueSignature", typeof(string));

            foreach (var sku in skuSignatures)
                table.Rows.Add(sku.SeqNo, sku.ValueSignature);

            // 3️⃣ Call SQL SP
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
    }
}
