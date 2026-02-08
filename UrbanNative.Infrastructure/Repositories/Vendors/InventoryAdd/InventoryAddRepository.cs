using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Vendors.Inventory;
using UrbanNative.Application.DTOs.Vendors.Products;
using UrbanNative.Application.Interfaces.Vendors.InventoryAdd;
using UrbanNative.Domain.Exceptions;
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
        // ===========Adjust Inventory SKU Single==========================
        // =================================================
        // Adjust Inventory (Single SKU)
        // =================================================

        public async Task<AdjustInventoryResultDto> AdjustInventoryAsync(
            int? skuId,
            int? addressId,
            string changeType,
            int quantity,
            string reason,
            int vendorUserId)
        {
            using var conn = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@SKUId", skuId);
            parameters.Add("@WarehouseId", addressId);
            parameters.Add("@ChangeType", changeType);
            parameters.Add("@Quantity", quantity);
            parameters.Add("@Reason", reason);
            parameters.Add("@VendorUserID", vendorUserId);

            var result = await conn.QuerySingleOrDefaultAsync<AdjustInventoryResultDto>(
                "sp_Vendor_AdjustInventory",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (result == null)
                throw new DomainValidationException("ERR1", "Inventory adjustment failed.");

            return new AdjustInventoryResultDto
            {
                OldStock = result.OldStock,
                NewStock = result.NewStock
            };
        }

        // ======================================================
        // GET Single SKUS FOR ADDING INVENTORY: Summary
        // ======================================================
        public async Task<SkuInventoryStockSummaryDto> GetSkuStockSummaryAsync(
        int vendorId,
        int productId,
        int skuId,
        int warehouseId)
        {
            using var conn = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@@VendorId", vendorId);
            parameters.Add("@ProductId", productId);
            parameters.Add("@SkuId", skuId);
            parameters.Add("@WarehouseId", warehouseId);

            /*
             IMPORTANT
             ---------
             We reuse the SAME query / SP logic already used in Part 3.1.
             If you already have a SP, replace the name below.
            */

            return await conn.QuerySingleOrDefaultAsync<SkuInventoryStockSummaryDto>(
                "sp_VendorSkuAddInventory_Summary",
                parameters,
                commandType: CommandType.StoredProcedure
            ) ?? new SkuInventoryStockSummaryDto();
        }

        // ===========END SKU Single==========================
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

            var parameters = new DynamicParameters();
            parameters.Add("@VendorId", vendorId);
            parameters.Add("@ProductId", productId);
            parameters.Add("@WarehouseId", warehouseId);
            parameters.Add(
                "@Items",
                dt.AsTableValuedParameter("dbo.InventoryAddSkuQty_TVP")
            );
            parameters.Add("@Remarks", remarks);

            await conn.ExecuteAsync(
                "sp_VendorInventoryAdd_Product_IN",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }

}
