using UrbanNative.Application.DTOs.Vendors;

namespace UrbanNative.Application.Interfaces.Vendors
{
    public interface IVendorSettingsRepository
    {
        Task<List<VendorSystemSettingDto>> GetAsync(int vendorId);
        Task UpdateAsync(int systemSettingId, decimal value, DateTime effectiveFrom, int vendorId);
        Task<List<VendorSystemSettingHistoryDto>> GetHistoryAsync( int vendorId, int systemSettingId);

    }

}
