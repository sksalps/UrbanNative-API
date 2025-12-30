using System.Text.Json;
using UrbanNative.Application.DTOs.AdminCategory;

namespace UrbanNative.Admin.Services
{
    public class AdminCategoryHSNService : IAdminCategoryHSNService
    {
        private readonly HttpClient _http;

        public AdminCategoryHSNService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public async Task<CategoryHSNDto?> GetByCategoryIdAsync(int categoryId)
        {
            var response = await _http.GetAsync(
                $"api/admin/categories/{categoryId}/hsn"
            );

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json) || json == "null")
                return null;

            return JsonSerializer.Deserialize<CategoryHSNDto>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }

        public async Task LinkOrUpdateAsync(int categoryId, int hsnId)
        {
            await _http.PostAsJsonAsync(
                $"api/admin/categories/{categoryId}/hsn",
                hsnId
            );
        }

        public async Task RemoveAsync(int categoryId)
        {
            await _http.DeleteAsync(
                $"api/admin/categories/{categoryId}/hsn"
            );
        }
    }
}