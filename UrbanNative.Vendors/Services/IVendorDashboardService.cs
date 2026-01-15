using UrbanNative.Application.DTOs.Vendors;

namespace UrbanNative.Vendors.Services
{
    public interface IVendorDashboardService
    {
        Task<VendorDashboardDto> GetDashboardAsync();
    }
}
