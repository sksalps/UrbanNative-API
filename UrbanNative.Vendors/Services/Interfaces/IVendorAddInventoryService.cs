using UrbanNative.Application.DTOs.Vendors.Inventory;

namespace UrbanNative.Vendors.Services.Interfaces
{
    public interface IVendorAddInventoryService
    {
        Task<InventoryInResponseDto> AddProductInventoryInAsync(    ProductInventoryInRequestDto request);
        Task<IReadOnlyList<ProductAddInventorySkuGridDto>> GetProductSkusForInventoryAsync(int productId, int warehouseId);
        Task<AddInventoryResultDto> AddSkuInventoryAsync(
        int skuId,
        int warehouseId,
        int quantity,
        string? remarks);

        Task<SkuInventoryStockSummaryDto> GetSkuStockSummaryAsync(
            int skuId,
            int warehouseId);
    }

}
