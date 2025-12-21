using System.Text.Json;
using UrbanNative.Application.DTOs.AdminCategory;
using static System.Net.WebRequestMethods;

namespace UrbanNative.Admin.Services
{
    public class AdminCategoryService : IAdminCategoryService
    {
        private readonly HttpClient _http;

        public AdminCategoryService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        // =========================
        // Admin – Category Listing
        // =========================
        public async Task<IEnumerable<AdminCategoryListDto>> GetCategoriesAsync()
        {
            return await _http.GetFromJsonAsync<IEnumerable<AdminCategoryListDto>>(
                "/api/admin/categories")
                ?? Enumerable.Empty<AdminCategoryListDto>();
        }

        // =========================
        // Admin – Single Category
        // =========================
        public async Task<AdminCategoryDetailDto?> GetByIdAsync(int categoryId)
        {
            var res = await _http.GetAsync($"/api/admin/categories/{categoryId}");

            if (!res.IsSuccessStatusCode)
                return null;

            return await res.Content.ReadFromJsonAsync<AdminCategoryDetailDto>();
        }

        // =========================
        // Admin – Create Category
        // =========================
        public async Task<string?> CreateAsync(AdminCategorySaveDto dto)
        {
            var res = await _http.PostAsJsonAsync("/api/admin/categories", dto);

            if (res.IsSuccessStatusCode)
                return null;

            var json = await res.Content.ReadAsStringAsync();
            return string.IsNullOrWhiteSpace(json)
                ? "Unable to create category"
                : JsonDocument.Parse(json).RootElement.GetProperty("message").GetString();
        }



        // =========================
        // Admin – Update Category
        // =========================

        public async Task<string?> UpdateAsync(int categoryId, AdminCategorySaveDto dto)
        {
            var res = await _http.PutAsJsonAsync(
                $"/api/admin/categories/{categoryId}", dto);

            if (res.IsSuccessStatusCode)
                return null;

            var json = await res.Content.ReadAsStringAsync();
            return string.IsNullOrWhiteSpace(json)
                ? "Unable to update category"
                : JsonDocument.Parse(json).RootElement.GetProperty("message").GetString();
        }


        // =========================
        // Admin – Activate / Deactivate Category
        // =========================

        public async Task<string?> ToggleActiveAsync(int categoryId)
        {
            var res = await _http.PostAsJsonAsync(
                "/api/admin/categories/activate", categoryId);

            if (res.IsSuccessStatusCode)
                return null;

            var json = await res.Content.ReadAsStringAsync();
            return string.IsNullOrWhiteSpace(json)
                ? "Unable to update category status"
                : JsonDocument.Parse(json).RootElement.GetProperty("message").GetString();
        }




    }
}
