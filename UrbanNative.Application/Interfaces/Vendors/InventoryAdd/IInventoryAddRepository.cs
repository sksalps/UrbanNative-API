using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Vendors.Inventory;

namespace UrbanNative.Application.Interfaces.Vendors.InventoryAdd
{
    public interface IInventoryAddRepository
    {
        
        Task AddProductInventoryInAsync( int vendorId,
            int productId,
            int warehouseId,
            List<ProductInventoryInItemDto> items,
            string? remarks);
        Task<IReadOnlyList<ProductAddInventorySkuGridDto>>GetProductInventorySkusAsync( int vendorId,
        int productId,
        int warehouseId);
        Task<SkuInventoryStockSummaryDto> GetSkuStockSummaryAsync( int vendorId,int productId,      int skuId,     int warehouseId);

        // Adjust Inventory IN / OUT
        Task<AdjustInventoryResultDto> AdjustInventoryAsync(
        int? skuId,
        int? warehouseId,
        string changeType,
        int quantity,
        string reason,
        int vendorUserId);
    }

}
