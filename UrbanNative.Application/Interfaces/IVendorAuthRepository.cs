using UrbanNative.Application.DTOs.Vendors;

namespace UrbanNative.Application.Interfaces
{
    public interface IVendorAuthRepository
    {
        Task<VendorLoginResultDto?> GetVendorForLoginAsync(string identifier);
        

        Task<bool> VerifyPasswordAsync(int vendorId, string password);

        Task<bool> UpdatePasswordAsync(int vendorId, string newPassword);

    }
}
