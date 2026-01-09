using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Admin.Services;
using UrbanNative.Application.DTOs.AdminOrders;

namespace UrbanNative.Admin.Pages.Orders
{
    public class OrderDetailsModel : PageModel
    {
        private readonly IAdminOrderService _orderService;

        public OrderDetailsModel(IAdminOrderService orderService)
        {
            _orderService = orderService;
        }

        public AdminOrderDetailsDto Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            if (orderId <= 0)
                return NotFound();

            Order = await _orderService.GetOrderDetailsAsync(orderId);

            if (Order == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnGetDownloadPdfAsync(int orderId)
        {
            var pdfBytes = await _orderService.DownloadOrderPdfAsync(orderId);

            return File(
                pdfBytes,
                "application/pdf",
                $"Order_{orderId}.pdf"
            );
        }
    }
}
