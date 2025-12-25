using System.Net.Http.Json;
using UrbanNative.Application.DTOs.AdminVariant;

namespace UrbanNative.Admin.Services
{
    public class AdminVariantService : IAdminVariantService
    {
        private readonly HttpClient _http;

        public AdminVariantService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        // =========================
        // Variant Listing
        // =========================
        public async Task<IEnumerable<AdminVariantListDto>> GetVariantsAsync(
            string? search,
            bool? isActive)
        {
            var query = new List<string>();

            if (!string.IsNullOrWhiteSpace(search))
                query.Add($"search={search}");

            if (isActive.HasValue)
                query.Add($"isActive={isActive}");

            var url = "api/admin/variants";
            if (query.Any())
                url += "?" + string.Join("&", query);

            return await _http.GetFromJsonAsync<IEnumerable<AdminVariantListDto>>(url)
                   ?? Enumerable.Empty<AdminVariantListDto>();
        }

        // =========================
        // Variant Details
        // =========================
        public async Task<AdminVariantDetailsDto> GetVariantAsync(int variantId)
        {
            return await _http.GetFromJsonAsync<AdminVariantDetailsDto>(
                $"api/admin/variants/{variantId}")
                ?? throw new Exception("Variant not found");
        }

        // =========================
        // Variant Values
        // =========================
        public async Task<IEnumerable<AdminVariantValueDto>> GetVariantValuesAsync(int variantId)
        {
            return await _http.GetFromJsonAsync<IEnumerable<AdminVariantValueDto>>(
                $"api/admin/variant-values/by-variant/{variantId}")
                ?? Enumerable.Empty<AdminVariantValueDto>();
        }

        // =========================
        // Create Variant
        // =========================

        public async Task CreateVariantAsync(string variantName)
        {
            var response = await _http.PostAsJsonAsync(
                "api/admin/variants",
                new { VariantName = variantName });

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(
                    string.IsNullOrWhiteSpace(error)
                        ? "Variant already exists."
                        : error);
            }

            response.EnsureSuccessStatusCode();
        }
        //Update Variant
        public async Task UpdateVariantAsync(int variantId, string variantName)
        {
            var response = await _http.PutAsJsonAsync(
                $"api/admin/variants/{variantId}",
                new { VariantName = variantName });

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(
                    string.IsNullOrWhiteSpace(error)
                        ? "Variant already exists."
                        : error);
            }

            response.EnsureSuccessStatusCode();
        }

        //UpdateVariantValueAsync
        public async Task UpdateVariantValueAsync(int variantValueId, string valueName)
        {
            var response = await _http.PutAsJsonAsync(
                $"api/admin/variant-values/{variantValueId}",
                new { ValueName = valueName });

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(
                    string.IsNullOrWhiteSpace(error)
                        ? "Variant value already exists."
                        : error);
            }

            response.EnsureSuccessStatusCode();
        }



        // =========================
        // Toggle Variant
        // =========================
        public async Task ToggleVariantAsync(int variantId)
        {
            var response = await _http.PatchAsync(
                $"api/admin/variants/{variantId}/toggle",
                null);

            response.EnsureSuccessStatusCode();
        }

        // =========================
        // Add Variant Value
        // =========================
        public async Task AddVariantValueAsync(int variantId, string valueName)
        {
            var response = await _http.PostAsJsonAsync(
                "api/admin/variant-values",
                new
                {
                    VariantID = variantId,
                    ValueName = valueName
                });

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(
                    string.IsNullOrWhiteSpace(error)
                        ? "Variant value already exists."
                        : error);
            }

            response.EnsureSuccessStatusCode();
        }


        // =========================
        // Toggle Variant Value
        // =========================
        public async Task ToggleVariantValueAsync(int variantValueId)
        {
            var response = await _http.PatchAsync(
                $"api/admin/variant-values/{variantValueId}/toggle",
                null);

            response.EnsureSuccessStatusCode();
        }
    }
}