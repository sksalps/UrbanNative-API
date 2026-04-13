using UrbanNative.Application.DTOs.Vendors;

namespace UrbanNative.Vendors.Services.Interfaces
{
    public interface IVendorWarehouseService
    {
        Task<List<VendorWarehouseListDto>> GetWarehousesAsync();
        Task<VendorWarehouseSaveDto> GetWarehouseByIdAsync(int? warehouseId);
        Task<ServiceResult> DeleteWarehouseAsync(int warehouseId);
        Task<ServiceResult> SetPrimaryWarehouseAsync(int warehouseId);
        Task SaveWarehouseAsync(VendorWarehouseSaveDto dto);
    }
}
