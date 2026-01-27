using System.Net.Http.Json;
using UrbanNative.Application.DTOs.Vendors.Inventory;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Services
{
    public class VendorInventoryService : IVendorInventoryService
    {
        private readonly HttpClient _http;

        public VendorInventoryService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient"); // ✅ SAME AS ADMIN
        }

        public async Task<VendorInventorySummaryDto> GetInventorySummaryAsync(
            int skuId,
            int? addressId,
            DateTime fromDate,
            DateTime toDate)
        {
            var resolvedAddressId = addressId ?? 0;

            var url =
                $"api/vendor/inventory/{skuId}/summary" +
                $"?addressId={addressId}" +
                $"&fromDate={fromDate:yyyy-MM-dd}" +
                $"&toDate={toDate:yyyy-MM-dd}";

            return await _http.GetFromJsonAsync<VendorInventorySummaryDto>(url)
                   ?? new VendorInventorySummaryDto();
        }

        public async Task<IReadOnlyList<VendorInventoryLogDto>> GetInventoryLogsAsync(
            int skuId,
            int? addressId,
            DateTime fromDate,
            DateTime toDate,
            int page,
            int pageSize)
        {
            // 🔒 Normalize before API call
            var resolvedAddressId = addressId ?? 0;
            var url =
                $"api/vendor/inventory/{skuId}/logs" +
                $"?addressId={addressId}" +
                $"&fromDate={fromDate:yyyy-MM-dd}" +
                $"&toDate={toDate:yyyy-MM-dd}" +
                $"&page={page}" +
                $"&pageSize={pageSize}";

            return await _http.GetFromJsonAsync<List<VendorInventoryLogDto>>(url)
                   ?? new List<VendorInventoryLogDto>();
        }
    }
}
