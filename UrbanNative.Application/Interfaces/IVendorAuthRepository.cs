using UrbanNative.Application.DTOs.Vendors;

namespace UrbanNative.Application.Interfaces
{
    public interface IVendorAuthRepository
    {
        Task<VendorLoginResultDto?> GetVendorForLoginAsync(string identifier);
    }
}
