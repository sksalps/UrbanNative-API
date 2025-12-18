using UrbanNative.Application.DTOs;

namespace UrbanNative.Admin.Services
{
    public class AdminProductService : IAdminProductService
    {
        private readonly HttpClient _http;

        public AdminProductService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        // =========================
        // Admin – Products Listing
        // =========================
        public async Task<IEnumerable<AdminProductDto>> GetProductsAsync(
            string? search,
            string? approvalStatus,
            bool? isActive)
        {
            var query = new List<string>();

            if (!string.IsNullOrWhiteSpace(search))
                query.Add($"search={Uri.EscapeDataString(search)}");

            if (!string.IsNullOrWhiteSpace(approvalStatus))
                query.Add($"approvalStatus={approvalStatus}");

            if (isActive.HasValue)
                query.Add($"isActive={isActive.Value}");

            var url = "/api/admin/products";
            if (query.Any())
                url += "?" + string.Join("&", query);

            return await _http.GetFromJsonAsync<IEnumerable<AdminProductDto>>(url)
                   ?? Enumerable.Empty<AdminProductDto>();
        }

        // =========================
        // Admin – Single Product
        // =========================
        public async Task<AdminProductDto?> GetByIdAsync(int productId)
        {
            var res = await _http.GetAsync($"/api/admin/products/{productId}");

            if (!res.IsSuccessStatusCode)
                return null;

            return await res.Content.ReadFromJsonAsync<AdminProductDto>();
        }


        // =========================
        // Admin – Actions
        // =========================
        public async Task ApproveAsync(int productId, string? remark)
        {
            var payload = new
            {
                Remark = remark
            };

            var res = await _http.PostAsJsonAsync(
                $"/api/admin/products/{productId}/approve",
                payload
            );

            res.EnsureSuccessStatusCode();
        }

        public async Task RejectAsync(int productId, string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Reject reason is required.");

            var payload = new
            {
                Reason = reason
            };

            var res = await _http.PostAsJsonAsync(
                $"/api/admin/products/{productId}/reject",
                payload
            );

            res.EnsureSuccessStatusCode();
        }

        public async Task ToggleActiveAsync(int productId)
        {
            var res = await _http.PostAsync(
                $"/api/admin/products/{productId}/toggle-active",
                null
            );

            res.EnsureSuccessStatusCode();
        }
    }
}