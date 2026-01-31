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
        //Task<List<ProductSkuSnapshotDto>> GetProductSkuSnapshotAsync(   int vendorId, int productId);

        Task AddProductInventoryInAsync(
            int vendorId,
            int productId,
            int warehouseId,
            List<ProductInventoryInItemDto> items,
            string? remarks);
        //Task<IReadOnlyList<ProductAddInventorySkuGridDto>>   GetProductInventorySkusAsync(int vendorId, int productId);
        Task<IReadOnlyList<ProductAddInventorySkuGridDto>>GetProductInventorySkusAsync( int vendorId,
        int productId,
        int warehouseId);

    }

}
