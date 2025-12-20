using UrbanNative.Application.DTOs.AdminVendor;

namespace UrbanNative.Application.Interfaces
{
    public interface IAdminVendorRepository
    {
        // =========================
        // Admin – Vendors Listing
        // =========================
        Task<IEnumerable<AdminVendorListDto>> GetAdminVendorsAsync(
            string? search,
            string? approvalStatus,
            bool? isActive);

        // =========================
        // Admin – Vendor Details
        // =========================
        Task<AdminVendorDetailDto?> GetAdminVendorByIdAsync(int vendorId);
        Task<VendorReferralInfoDto?> GetVendorReferralAsync(int vendorId);

        Task<IEnumerable<VendorMediaDto>> GetVendorMediaAsync(int vendorId);


        // =========================
        // Admin – Actions
        // =========================
        Task<bool> UpdateVendorApprovalAsync(
            int vendorId,
            int adminId,
            string approvalStatus,
            string? reason);

        Task<bool> ToggleVendorActiveAsync(int vendorId);


    }
}
