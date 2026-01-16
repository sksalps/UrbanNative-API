using UrbanNative.Application.DTOs.Vendors;

namespace UrbanNative.Vendors.Services.Interfaces
{
    public interface IVendorProfileService
    {
        Task<VendorProfileDto> GetProfileAsync();
        Task UpdateProfileAsync(VendorProfileDto dto);
    }
}
