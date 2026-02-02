using UrbanNative.Application.DTOs.Vendors.Inventory;

namespace UrbanNative.Application.Interfaces.UseCases.VendorInventoryAdd
{
    //======================Product Level Inventory Add=========================
    public interface IAddInventoryInUseCase
    {
        Task<InventoryInResponseDto> ExecuteAsync(  int vendorId,    ProductInventoryInRequestDto request);
        Task<IReadOnlyList<ProductAddInventorySkuGridDto>> ExecuteAsync(int vendorId, int productId, int warehouseId);
        Task<AddInventoryResultDto> ExecuteAsyncAddStockSKU(int vendorId,
                int skuId,
                int warehouseId,
                int quantity,
                string? remarks);
        Task<SkuInventoryStockSummaryDto> ExecuteAsyncSummary(int vendorId, int skuId, int warehouseId);
    }

}
