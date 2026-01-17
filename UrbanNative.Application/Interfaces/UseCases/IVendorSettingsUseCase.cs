using UrbanNative.Application.DTOs.Vendors;

namespace UrbanNative.Application.Interfaces.UseCases
{
    public interface IVendorSettingsUseCase
    {
        /// <summary>
        /// Get all applicable system settings for Vendor
        /// (editable + admin-managed, current effective values)
        /// </summary>
        Task<List<VendorSystemSettingDto>> GetAsync(int vendorId);

        /// <summary>
        /// Update a single vendor-editable system setting
        /// (one row at a time, history-safe)
        /// </summary>
        Task UpdateAsync(VendorSystemSettingUpdateDto dto,  int vendorId);

        Task<List<VendorSystemSettingHistoryDto>> GetHistoryAsync(int vendorId, int systemSettingId);

    }
}
