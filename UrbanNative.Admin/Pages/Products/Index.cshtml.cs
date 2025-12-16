using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace UrbanNative.Admin.Pages.Products
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // =============================
        // Filters (Query Params)
        // =============================
        public string? Search { get; set; }
        public string? ApprovalStatus { get; set; }
        public bool? IsActive { get; set; }

        // =============================
        // Products List (UI Model)
        // =============================
        public List<ProductRowVm> Products { get; set; } = new();

        public async Task OnGetAsync(string? search, string? approvalStatus, bool? isActive)
        {
            Search = search;
            ApprovalStatus = approvalStatus;
            IsActive = isActive;

            var client = _httpClientFactory.CreateClient("ApiClient");

            var query = new List<string>();
            if (!string.IsNullOrWhiteSpace(search))
                query.Add($"search={search}");
            if (!string.IsNullOrWhiteSpace(approvalStatus))
                query.Add($"approvalStatus={approvalStatus}");
            if (isActive.HasValue)
                query.Add($"isActive={isActive.Value}");

            var url = "/api/admin/products";
            if (query.Any())
                url += "?" + string.Join("&", query);

            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            Products = JsonSerializer.Deserialize<List<ProductRowVm>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }
    }

    // =============================
    // ViewModel for Products Table
    // =============================
    public class ProductRowVm
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string VendorName { get; set; } = string.Empty;
        public decimal MRP { get; set; }
        public decimal DiscountPrice { get; set; }
        public int Stock { get; set; }
        public string ApprovalStatus { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
