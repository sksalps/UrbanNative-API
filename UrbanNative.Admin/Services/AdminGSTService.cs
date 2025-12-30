using UrbanNative.Application.DTOs.AdminGST;
using System.Text.Json;

namespace UrbanNative.Admin.Services
{
    public class AdminGSTService
    {
        private readonly HttpClient _http;

        public AdminGSTService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public async Task<IEnumerable<AdminGSTListDto>> GetGSTAsync(
            decimal? gstPercentage,
            bool? isActive)
        {
            return await _http.GetFromJsonAsync<IEnumerable<AdminGSTListDto>>(
                $"api/admin/gst?gstPercentage={gstPercentage}&isActive={isActive}");
        }

        public async Task ToggleGSTAsync(int gstId, bool isActive)
        {
            await _http.PostAsync(
                $"api/admin/gst/{gstId}/toggle?isActive={isActive}",
                null);
        }
    }
}
