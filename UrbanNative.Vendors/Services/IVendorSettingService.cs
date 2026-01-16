using UrbanNative.Application.DTOs.Vendors;

public interface IVendorSettingsService
{
    Task<VendorSettingsDto> GetSettingsAsync(int vendorId);
    Task SaveSettingsAsync(int vendorId, VendorSettingsDto dto);
}
