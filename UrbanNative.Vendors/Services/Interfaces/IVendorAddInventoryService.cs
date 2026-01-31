using UrbanNative.Application.DTOs.Vendors.Inventory;

namespace UrbanNative.Vendors.Services.Interfaces
{
    public interface IVendorAddInventoryService
    {
        Task<InventoryInResponseDto> AddProductInventoryInAsync(    ProductInventoryInRequestDto request);
        Task<IReadOnlyList<ProductAddInventorySkuGridDto>> GetProductSkusForInventoryAsync(int productId, int warehouseId);

    }

}
