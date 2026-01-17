using System.Net.Http.Json;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Services
{
    public class VendorSettingsService : IVendorSettingsService
    {
        
        private readonly HttpClient _http;

        public VendorSettingsService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }
        public async Task<List<VendorSystemSettingDto>> GetAsync()
        {
            return await _http.GetFromJsonAsync<List<VendorSystemSettingDto>>(
                $"/api/vendor/settings");
        }

        public async Task UpdateAsync(VendorSystemSettingUpdateDto dto)
        {
            var res = await _http.PutAsJsonAsync(
                $"/api/vendor/settings", dto);

            res.EnsureSuccessStatusCode();
        }
        // ✅ HISTORY SUPPORT 
        public async Task<List<VendorSystemSettingHistoryDto>> GetHistoryAsync(
            int systemSettingId)
        {
            return await _http.GetFromJsonAsync<List<VendorSystemSettingHistoryDto>>(
                $"/api/vendor/settings/{systemSettingId}/history");
        }
    }
}
