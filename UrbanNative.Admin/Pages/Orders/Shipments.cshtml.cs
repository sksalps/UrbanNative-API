using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Admin.Services;
using UrbanNative.Application.DTOs.AdminOrders;

namespace UrbanNative.Admin.Pages.Orders
{
    public class ShipmentsModel : PageModel
    {
        private readonly IAdminOrderService _orderService;

        public ShipmentsModel(IAdminOrderService orderService)
        {
            _orderService = orderService;
        }

        public AdminOrderShipmentDetailsDto Data { get; set; }

        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            if (orderId <= 0)
                return NotFound();

            Data = await _orderService.GetOrderShipmentDetailsAsync(orderId);

            if (Data == null)
                return NotFound();

            return Page();
        }
    }
}