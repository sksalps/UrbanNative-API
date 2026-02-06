using Microsoft.AspNetCore.Mvc.Rendering;
using UrbanNative.Application.DTOs.Vendors.Inventory;
namespace UrbanNative.Vendors.Services.Interfaces
{
    //Use in Inventory Summary and Inventory Logs Pages in Vendor Dashboard
    public interface IVendorInventoryService
    {
        Task<VendorInventorySummaryDto> GetInventorySummaryAsync(
            int skuId,
            int? addressId,
            DateTime fromDate,
            DateTime toDate);

        Task<IReadOnlyList<VendorInventoryLogDto>> GetInventoryLogsAsync(
            int skuId,
            int? addressId,
            DateTime fromDate,
            DateTime toDate,
            int page,
            int pageSize);

        
    }
}
