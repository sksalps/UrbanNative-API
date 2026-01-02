using System.Net.Http.Json;
using UrbanNative.Application.DTOs.AdminInventory;

namespace UrbanNative.Admin.Services
{
    public class AdminInventoryService : IAdminInventoryService
    {
        private readonly HttpClient _http;

        public AdminInventoryService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        // ============================================
        // Inventory List
        // ============================================
        public async Task<IEnumerable<AdminInventoryListDto>> GetInventoryAsync(
            string? search,
            int? categoryId,
            bool? lowStockOnly,
            bool? isActive)
        {
            var query = new List<string>();

            if (!string.IsNullOrWhiteSpace(search))
                query.Add($"search={Uri.EscapeDataString(search)}");

            if (categoryId.HasValue)
                query.Add($"categoryId={categoryId.Value}");

            if (lowStockOnly.HasValue)
                query.Add($"lowStockOnly={lowStockOnly.Value}");

            if (isActive.HasValue)
                query.Add($"isActive={isActive.Value}");

            var url = "/api/admin/inventory";
            if (query.Any())
                url += "?" + string.Join("&", query);

            return await _http.GetFromJsonAsync<IEnumerable<AdminInventoryListDto>>(url)
                   ?? Enumerable.Empty<AdminInventoryListDto>();
        }

        // ============================================
        // Inventory Summary (Product Header)
        // ============================================
        public async Task<AdminInventorySummaryDto?> GetInventorySummaryAsync(int productId)
        {
            return await _http.GetFromJsonAsync<AdminInventorySummaryDto>(
                $"/api/admin/inventory/{productId}/summary"
            );
        }

        // ============================================
        // Inventory SKU Drill-Down
        // ============================================
        public async Task<IEnumerable<AdminInventorySkuDto>> GetInventorySkusAsync(int productId)
        {
            return await _http.GetFromJsonAsync<IEnumerable<AdminInventorySkuDto>>(
                $"/api/admin/inventory/{productId}/skus"
            ) ?? Enumerable.Empty<AdminInventorySkuDto>();
        }
    }
}
