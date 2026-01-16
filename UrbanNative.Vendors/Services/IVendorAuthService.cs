using UrbanNative.Application.DTOs.Vendors;

namespace UrbanNative.Application.Interfaces
{
    public interface IVendorAuthService
    {
        Task<VendorLoginResponseDto?> LoginAsync(VendorLoginRequestDto request);
        //Task<bool> ChangePasswordAsync(int vendorId, string currentPassword, string newPassword);
    }
}
