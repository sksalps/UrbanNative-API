using UrbanNative.Application.DTOs.Vendors;

namespace UrbanNative.Application.Interfaces
{
    public interface IVendorProfileRepository
    {
        /// <summary>
        /// Get vendor profile for given vendorId
        /// </summary>
        Task<VendorProfileDto> GetProfileAsync(int vendorId);

        /// <summary>
        /// Update vendor profile for given vendorId
        /// </summary>
        Task UpdateProfileAsync(int vendorId, VendorProfileDto profile);
    }
}
