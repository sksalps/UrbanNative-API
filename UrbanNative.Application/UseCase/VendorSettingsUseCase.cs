using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces.UseCases;
using UrbanNative.Application.Interfaces.Vendors;

namespace UrbanNative.Application.UseCases.Vendors
{
    public class VendorSettingsUseCase        : IVendorSettingsUseCase
    {
        private readonly IVendorSettingsRepository _repo;

        public VendorSettingsUseCase(    IVendorSettingsRepository repo)
        {
            _repo = repo;
        }

        public Task<List<VendorSystemSettingDto>> GetAsync(int vendorId)
        {
            // VendorId not required yet (global settings),
            // but kept for future per-vendor overrides
            return _repo.GetAsync(vendorId);
        }

        public async Task UpdateAsync(
            VendorSystemSettingUpdateDto dto,
            int vendorId)
        {
            // 🔒 Business rule enforcement
            var allSettings = await _repo.GetAsync(vendorId);
            var setting = allSettings
                .FirstOrDefault(x => x.SystemSettingId == dto.SystemSettingId);

            if (setting == null)
                throw new InvalidOperationException("Invalid setting");

            if (setting.OnlyAdminEditable)
                throw new UnauthorizedAccessException(
                    "This setting is admin-managed");

            await _repo.UpdateAsync(
                dto.SystemSettingId,
                dto.NewValue,
                dto.EffectiveFrom,
                vendorId);
        }

        public async Task<List<VendorSystemSettingHistoryDto>> GetHistoryAsync(
            int vendorId,
            int systemSettingId)
        {
            return await _repo.GetHistoryAsync(vendorId, systemSettingId);
        }


    }
}
