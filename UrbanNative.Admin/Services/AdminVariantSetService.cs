using UrbanNative.Application.DTOs.AdminCategory;
using UrbanNative.Application.DTOs.AdminVariantSet;

namespace UrbanNative.Admin.Services
{
    public class AdminVariantSetService : IAdminVariantSetService
    {
        private readonly HttpClient _http;

        public AdminVariantSetService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        // =========================
        // Variant Sets
        // =========================

        public async Task<IEnumerable<AdminVariantSetListDto>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<IEnumerable<AdminVariantSetListDto>>(
                "api/admin/variant-sets") ?? [];
        }

        public async Task MoveVariantAsync(int variantSetVariantId, string direction)
        {
            var response = await _http.PostAsJsonAsync(
                $"api/admin/variant-set-variants/{variantSetVariantId}/move",
                new { Direction = direction });

            response.EnsureSuccessStatusCode();
        }



        public async Task<AdminVariantSetDetailsDto?> GetDetailsAsync(int variantSetId)
        {
            return await _http.GetFromJsonAsync<AdminVariantSetDetailsDto>(
                $"api/admin/variant-sets/{variantSetId}");
        }

        public async Task CreateAsync(string variantSetName)
        {
            var response = await _http.PostAsJsonAsync(
                "api/admin/variant-sets",
                new { VariantSetName = variantSetName });

            await EnsureSuccess(response);
        }

        public async Task UpdateAsync(int variantSetId, string variantSetName)
        {
            var response = await _http.PutAsJsonAsync(
                $"api/admin/variant-sets/{variantSetId}",
                new { VariantSetName = variantSetName });

            await EnsureSuccess(response);
        }

        public async Task ToggleStatusAsync(int variantSetId)
        {
            var response = await _http.PostAsync(
                $"api/admin/variant-sets/{variantSetId}/toggle",
                null);

            await EnsureSuccess(response);
        }

        // =========================
        // Variants inside Set
        // =========================

        public async Task<IEnumerable<AdminVariantInsideSetDto>> GetVariantsAsync(int variantSetId)
        {
            return await _http.GetFromJsonAsync<IEnumerable<AdminVariantInsideSetDto>>(
                $"api/admin/variant-sets/{variantSetId}/variants") ?? [];
        }

        public async Task AddVariantAsync(int variantSetId, int variantId)
        {
            var response = await _http.PostAsJsonAsync(
                $"api/admin/variant-sets/{variantSetId}/variants",
                new { VariantID = variantId });

            await EnsureSuccess(response);
        }

        public async Task RemoveVariantAsync(int variantSetVariantId)
        {
            var response = await _http.DeleteAsync(
                $"api/admin/variant-set-variants/{variantSetVariantId}");

            await EnsureSuccess(response);
        }

        public async Task UpdateVariantOrderAsync(int variantSetVariantId, int newSortOrder)
        {
            var response = await _http.PutAsJsonAsync(
                $"api/admin/variant-set-variants/{variantSetVariantId}/order",
                new { SortOrder = newSortOrder });

            await EnsureSuccess(response);
        }

        // =========================
        // Category Assignment
        // =========================

        public async Task<IEnumerable<AdminVariantSetCategoryDto>> GetAssignedCategoriesAsync(int variantSetId)
        {
            return await _http.GetFromJsonAsync<IEnumerable<AdminVariantSetCategoryDto>>(
                $"api/admin/variant-sets/{variantSetId}/categories") ?? [];
        }

        public async Task AssignCategoryAsync(int variantSetId, int categoryId)
        {
            var response = await _http.PostAsJsonAsync(
                $"api/admin/variant-sets/{variantSetId}/categories",
                new { CategoryID = categoryId });

            await EnsureSuccess(response);
        }
        public async Task<IEnumerable<AdminCategoryListDto>> GetAvailableCategoriesAsync(int variantSetId)
        {
            return await _http.GetFromJsonAsync<IEnumerable<AdminCategoryListDto>>(
                $"api/admin/variant-sets/{variantSetId}/categories/available")
                ?? [];
        }
        public async Task RemoveCategoryAsync(int categoryVariantSetId)
        {
            var response = await _http.DeleteAsync(
                $"api/admin/variant-set-categories/{categoryVariantSetId}");

            await EnsureSuccess(response);
        }

        // =========================
        // Shared helper
        // =========================

        private static async Task EnsureSuccess(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(error);
            }

            response.EnsureSuccessStatusCode();
        }
    }
}
