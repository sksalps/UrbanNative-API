using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;
using UrbanNative.Application.DTOs.Vendors.Orders;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly IVendorOrderService _orderService;

        public IndexModel(IVendorOrderService orderService)
        {
            _orderService = orderService;
        }

        // -----------------------------
        // Bind Filter
        // -----------------------------
        [BindProperty]
        public VendorOrderListFilterDto Filter { get; set; } = new();

        // -----------------------------
        // View Models
        // -----------------------------
        public IReadOnlyList<VendorOrderListDto> Orders { get; set; } = [];
        public VendorOrderSummaryDto Summary { get; set; } = new();

        // -----------------------------
        // GET
        // -----------------------------
        public async Task OnGetAsync()
        {
            SetDefaultDateRange();
            await LoadDataAsync();
        }

        // -----------------------------
        // POST (Search)
        // -----------------------------
        public async Task<IActionResult> OnPostSearchAsync()
        {
            NormalizeFilters();
            await LoadDataAsync();
            return Page();

        }

        // -----------------------------
        // Load Data
        // -----------------------------
        private async Task LoadDataAsync()
        {
            Orders = await _orderService.GetOrdersAsync(Filter);
            Summary = await _orderService.GetSummaryAsync(Filter);
        }

        // -----------------------------
        // Default Date Logic
        // -----------------------------
        private void SetDefaultDateRange()
        {
            if (!Filter.FromDate.HasValue && !Filter.ToDate.HasValue)
            {
                Filter.FromDate = DateTime.Today.AddMonths(-1);
                Filter.ToDate = DateTime.Today;
            }
        }

        // -----------------------------
        // Highlight helper
        // -----------------------------
        public string Highlight(string? source)
        {
            if (string.IsNullOrWhiteSpace(source) ||
                string.IsNullOrWhiteSpace(Filter.SkuOrProduct))
                return source ?? string.Empty;

            return Regex.Replace(
                source,
                Regex.Escape(Filter.SkuOrProduct),
                m => $"<mark>{m.Value}</mark>",
                RegexOptions.IgnoreCase
            );
        }

        public async Task<IActionResult> OnGetSkuProductSuggestionsAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return new JsonResult(Array.Empty<VendorSkuProductSuggestionDto>());

            var result = await _orderService.GetSkuProductSuggestionsAsync(term);

            return new JsonResult(result);
        }


        // -----------------------------
        // Normalize Filters (CRITICAL)
        // -----------------------------
        private void NormalizeFilters()
        {
            // 🔴 Normalize dropdown values
            Filter.PaymentStatus = string.IsNullOrWhiteSpace(Filter.PaymentStatus)
                ? null
                : Filter.PaymentStatus;

            Filter.ShipmentStatus = string.IsNullOrWhiteSpace(Filter.ShipmentStatus)
                ? null
                : Filter.ShipmentStatus;

            Filter.RTOFilter = string.IsNullOrWhiteSpace(Filter.RTOFilter)
                ? "ALL"
                : Filter.RTOFilter;

            // -----------------------------
            // 1️⃣ Order No override
            // -----------------------------
            if (!string.IsNullOrWhiteSpace(Filter.OrderNo))
            {
                Filter.OrderNo = Filter.OrderNo.Trim();
                Filter.FromDate = null;
                Filter.ToDate = null;
                Filter.SkuOrProduct = null;
                Filter.SearchType = null;
                return;
            }

            // -----------------------------
            // 2️⃣ SKU / Product override
            // -----------------------------
            if (!string.IsNullOrWhiteSpace(Filter.SkuOrProduct))
            {
                Filter.SkuOrProduct = Filter.SkuOrProduct.Trim();

                bool looksLikeSku =           !Filter.SkuOrProduct.Contains(' ')             && Filter.SkuOrProduct.Any(char.IsDigit);

                Filter.SearchType = looksLikeSku ? "SKU" : "PRODUCT";

                Filter.FromDate = null;
                Filter.ToDate = null;
                return;
            }
            else
            {
                Filter.SearchType = null;
            }

            // -----------------------------
            // 3️⃣ Default Date
            // -----------------------------
            if (!Filter.FromDate.HasValue && !Filter.ToDate.HasValue)
            {
                Filter.FromDate = DateTime.Today.AddMonths(-1);
                Filter.ToDate = DateTime.Today;
            }
        }
    }
}
