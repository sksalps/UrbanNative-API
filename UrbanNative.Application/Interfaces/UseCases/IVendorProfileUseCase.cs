using UrbanNative.Application.DTOs.Common;
using UrbanNative.Application.DTOs.Vendors;

namespace UrbanNative.Application.Interfaces.UseCases
{
    public interface IVendorProfileUseCase
    {
        Task<VendorProfileDto> GetProfileAsync(int vendorId);
        Task UpdateProfileAsync(int vendorId, VendorProfileDto profile);
    }
}
