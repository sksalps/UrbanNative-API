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

        [BindProperty(SupportsGet = true)]
        public VendorOrderListFilterDto Filter { get; set; } = new();

        public IReadOnlyList<VendorOrderListDto> Orders { get; set; } = [];
        public VendorOrderSummaryDto Summary { get; set; } = new();

        /* ===============================
           GET – Handles normal + deeplink
           =============================== */
        public async Task OnGetAsync()
        {
            NormalizeFilters(isFromDeeplink: true);
            await LoadDataAsync();
        }

        /* ===============================
           POST – Search button
           =============================== */
        public async Task<IActionResult> OnPostSearchAsync()
        {
            NormalizeFilters(isFromDeeplink: false);
            await LoadDataAsync();
            return Page();
        }

        /* ===============================
           Data Load
           =============================== */
        private async Task LoadDataAsync()
        {
            Orders = await _orderService.GetOrdersAsync(Filter);
            Summary = await _orderService.GetSummaryAsync(Filter);
        }

        /* ===============================
           Highlight helper
           =============================== */
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

        /* ===============================
           Filter Normalization (CORE)
           =============================== */
        private void NormalizeFilters(bool isFromDeeplink)
        {
            // Normalize empty dropdowns
            Filter.PaymentStatus =
                string.IsNullOrWhiteSpace(Filter.PaymentStatus)
                    ? null
                    : Filter.PaymentStatus;

            Filter.ShipmentStatus =
                string.IsNullOrWhiteSpace(Filter.ShipmentStatus)
                    ? null
                    : Filter.ShipmentStatus;

            Filter.RTOFilter =
                string.IsNullOrWhiteSpace(Filter.RTOFilter)
                    ? "ALL"
                    : Filter.RTOFilter;

            /* -----------------------------
               Order No override
               ----------------------------- */
            if (!string.IsNullOrWhiteSpace(Filter.OrderNo))
            {
                Filter.OrderNo = Filter.OrderNo.Trim();
                Filter.FromDate = null;
                Filter.ToDate = null;
                Filter.SkuOrProduct = null;
                Filter.SearchType = null;
                return;
            }

            /* -----------------------------
               SKU / Product override
               ----------------------------- */
            // Always recompute SearchType for SKU / Product
            Filter.SearchType = null;

            if (!string.IsNullOrWhiteSpace(Filter.SkuOrProduct))
            {
                Filter.SkuOrProduct = Filter.SkuOrProduct.Trim();

                bool looksLikeSku =
                    !Filter.SkuOrProduct.Contains(' ')
                    && Filter.SkuOrProduct.Any(char.IsDigit);

                Filter.SearchType = looksLikeSku ? "SKU" : "PRODUCT";

                Filter.FromDate = null;
                Filter.ToDate = null;
                return;
            }

            /* -----------------------------
               ShipmentStatus from deeplink
               Supports single or CSV
               ----------------------------- */
            if (!string.IsNullOrWhiteSpace(Filter.ShipmentStatus))
            {
                // Example: READY,SHIPPED
                Filter.ShipmentStatus =
                    Filter.ShipmentStatus
                          .Replace(" ", "")
                          .ToUpper();
            }

            /* -----------------------------
               Default Date (ONLY if needed)
               ----------------------------- */
            bool hasAnyFilter =
                !string.IsNullOrWhiteSpace(Filter.OrderNo) ||
                !string.IsNullOrWhiteSpace(Filter.SkuOrProduct) ||
                !string.IsNullOrWhiteSpace(Filter.ShipmentStatus) ||
                !string.IsNullOrWhiteSpace(Filter.PaymentStatus) ;

            if (!Filter.FromDate.HasValue &&
                !Filter.ToDate.HasValue &&
                !hasAnyFilter)
            {
                Filter.FromDate = DateTime.Today.AddMonths(-1);
                Filter.ToDate = DateTime.Today;
            }
        }
    }
}
