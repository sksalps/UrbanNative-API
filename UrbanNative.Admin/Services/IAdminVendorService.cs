using UrbanNative.Application.DTOs.AdminVendor;

namespace UrbanNative.Admin.Services
{
    public interface IAdminVendorService
    {
        // =========================
        // Admin – Vendors Listing
        // =========================
        Task<IEnumerable<AdminVendorListDto>> GetVendorsAsync(
            string? search,
            string? approvalStatus,
            bool? isActive);

        // =========================
        // Admin – Single Vendor
        // =========================
        Task<AdminVendorDetailDto?> GetByIdAsync(int vendorId);
        Task<VendorReferralInfoDto?> GetVendorReferralAsync(int vendorId);
        Task<IEnumerable<VendorMediaDto>> GetVendorMediaAsync(int vendorId);
        // =========================
        // Admin – Actions
        // =========================
        Task UpdateApprovalAsync(
            int vendorId,
            string approvalStatus,
            string? reason);

        Task ToggleActiveAsync(int vendorId);


    }
}
