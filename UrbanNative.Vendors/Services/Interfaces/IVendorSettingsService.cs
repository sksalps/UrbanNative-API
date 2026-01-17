using UrbanNative.Application.DTOs.Vendors;

namespace UrbanNative.Vendors.Services.Interfaces
{
    public interface IVendorSettingsService
    {
        Task<List<VendorSystemSettingDto>> GetAsync();
        Task UpdateAsync(VendorSystemSettingUpdateDto dto);
        Task<List<VendorSystemSettingHistoryDto>> GetHistoryAsync(int systemSettingId);
    }
}

