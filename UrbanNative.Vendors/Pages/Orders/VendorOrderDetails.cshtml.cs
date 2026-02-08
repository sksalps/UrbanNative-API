using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.Vendors.Orders;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Pages.Orders
{
    public class VendorOrderDetailsModel : PageModel
    {
        private readonly IVendorOrderService _vendorOrderService;

        public VendorOrderDetailsModel(IVendorOrderService vendorOrderService)
        {
            _vendorOrderService = vendorOrderService;
        }

        // ===============================
        // Query Parameters
        // ===============================
        [BindProperty(SupportsGet = true)]
        public int? OrderId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? ShipmentId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? ReturnId { get; set; }

        // ===============================
        // View Model
        // ===============================
        public VendorOrderDetailsDto OrderDetails { get; private set; }

        // ===============================
        // UI State
        // ===============================
        public bool HasError { get; private set; }
        public string ErrorMessage { get; private set; }

        // ===============================
        // GET
        // ===============================
        public async Task<IActionResult> OnGetAsync()
        {
            // 🔐 Client-side guard
            if (OrderId == null && ShipmentId == null && ReturnId == null)
            {
                HasError = true;
                ErrorMessage = "Invalid order reference.";
                return Page();
            }

            try
            {
                OrderDetails = await _vendorOrderService.GetOrderDetailsAsync(
                    OrderId,
                    ShipmentId,
                    ReturnId
                );

                // Extremely defensive (should never happen)
                if (OrderDetails?.OrderSummary == null)
                {
                    HasError = true;
                    ErrorMessage = "Order details could not be loaded.";
                }

                return Page();
            }
            catch (HttpRequestException ex)
            {
                // API not reachable / 500
                HasError = true;
                ErrorMessage = "Unable to load order details. Please try again.";
                return Page();
            }
            catch (Exception ex)
            {
                // Fallback safety
                HasError = true;
                ErrorMessage = "Something went wrong while loading the order.";
                return Page();
            }
        }

        // ===============================
        // Helper Flags for UI
        // ===============================
        public bool HasItems =>
            OrderDetails?.Items != null && OrderDetails.Items.Any();

        public bool HasReturns =>
            OrderDetails?.Returns != null && OrderDetails.Returns.Any();
        public static string DisplayOrDash(string? value)
            => string.IsNullOrWhiteSpace(value) ? "—" : value;

        public static string DisplayOrDash(DateTime? value)
            => value.HasValue ? value.Value.ToString("dd MMM yyyy") : "—";

    }
}
