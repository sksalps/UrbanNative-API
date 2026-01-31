using UrbanNative.Application.DTOs.Vendors.Inventory;

namespace UrbanNative.Application.Interfaces.UseCases.VendorInventoryAdd
{
    //======================Product Level Inventory Add=========================
    public interface IAddInventoryInUseCase
    {
        Task<InventoryInResponseDto> ExecuteAsync(  int vendorId,    ProductInventoryInRequestDto request);
        Task<IReadOnlyList<ProductAddInventorySkuGridDto>> ExecuteAsync(int vendorId, int productId, int warehouseId);
    }

}
