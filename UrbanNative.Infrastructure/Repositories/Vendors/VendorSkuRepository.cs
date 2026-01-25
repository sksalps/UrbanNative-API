using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Vendors.Products;
using UrbanNative.Application.Interfaces.Vendors;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories.Vendors
{
    public class VendorSkuRepository : IVendorSkuRepository
    {

        private readonly SqlConnectionFactory _connectionFactory;

        public VendorSkuRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<List<VendorSkuGridDto>> GetSkuGridAsync(int productId, int vendorId)
        {
            using var conn = _connectionFactory.CreateConnection();
            return (await conn.QueryAsync<VendorSkuGridDto>(
                "SP_Vendor_ProductSKU_Grid",
                new { ProductID = productId, VendorID = vendorId },
                commandType: CommandType.StoredProcedure)).ToList();
        }
        public async Task<VendorSkuHeaderDto> GetSkuHeaderAsync(int productId)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QuerySingleAsync<VendorSkuHeaderDto>(
                "SP_Vendor_ProductSKU_Header",
                new { ProductID = productId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task SaveSkusAsync(int vendorId, List<VendorSkuSaveDto> skus)
        {
            var table = new DataTable();
            table.Columns.Add("SKUId", typeof(int));
            table.Columns.Add("Price", typeof(decimal));
            table.Columns.Add("Stock", typeof(int));
            table.Columns.Add("IsActive", typeof(bool));

            foreach (var s in skus)
                table.Rows.Add(s.SKUId, s.Price, s.Stock, s.IsActive);

            using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(
                "SP_Vendor_SKU_BulkSave",
                new { VendorID = vendorId, SKUs = table.AsTableValuedParameter("TVP_VendorSkuUpdate") },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
