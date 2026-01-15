using UrbanNative.Application.DTOs.Vendors;

namespace UrbanNative.Application.Interfaces
{
    public interface IVendorDashboardRepository
    {
        Task<VendorDashboardDto> GetVendorDashboardAsync(int vendorId);
    }

}
