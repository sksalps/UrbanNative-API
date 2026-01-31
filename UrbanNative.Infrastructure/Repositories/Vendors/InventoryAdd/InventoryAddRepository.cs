using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Vendors.Inventory;
using UrbanNative.Application.DTOs.Vendors.Products;
using UrbanNative.Application.Interfaces.Vendors.InventoryAdd;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories.Vendors.InventoryAdd
{
    public class InventoryAddRepository : IInventoryAddRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public InventoryAddRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IReadOnlyList<ProductAddInventorySkuGridDto>>GetProductInventorySkusAsync(int vendorId, int productId, int warehouseId)
        {
            using var conn = _connectionFactory.CreateConnection();

            var result = await conn.QueryAsync<ProductAddInventorySkuGridDto>(
            "sp_ProductInventorySkuGrid_Get",
            new
            {
                VendorId = vendorId,
                ProductId = productId,
                WarehouseId = warehouseId   // 🔥 REQUIRED
            },
            commandType: CommandType.StoredProcedure);


            return result.ToList();
        }
/*
        public async Task<List<ProductSkuSnapshotDto>> GetProductSkuSnapshotAsync(
            int vendorId, int productId)
        {
            using var conn = _connectionFactory.CreateConnection();

            return (await conn.QueryAsync<ProductSkuSnapshotDto>(
                "sp_ProductSkuSnapshot_Get11",
                new { VendorId = vendorId, ProductId = productId },
                commandType: CommandType.StoredProcedure)).ToList();          
        }
*/
        public async Task AddProductInventoryInAsync(
            int vendorId,
            int productId,
            int warehouseId,
            List<ProductInventoryInItemDto> items,
            string? remarks)
        {
            using var conn = _connectionFactory.CreateConnection();

            var dt = new DataTable();
            dt.Columns.Add("SKUId", typeof(int));
            dt.Columns.Add("Quantity", typeof(int));

            foreach (var item in items)
            {
                dt.Rows.Add(item.SKUId, item.Quantity);
            }

            await conn.ExecuteAsync(
             "sp_VendorInventoryAdd_Product_IN",
             new
             {
                 VendorId = vendorId,
                 ProductId = productId,
                 WarehouseId = warehouseId,
                 Items = dt.AsTableValuedParameter("dbo.InventoryAddSkuQty_TVP"),
                 Remarks = remarks,
                 CreatedBy = vendorId
             },
             commandType: CommandType.StoredProcedure);
        }
    }

}
