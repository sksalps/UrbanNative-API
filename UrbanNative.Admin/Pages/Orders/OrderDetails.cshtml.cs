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

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
                return NotFound();

            Order = await _orderService.GetOrderDetailsAsync(id);

            if (Order == null)
                return NotFound();

            return Page();
        }
    }
}
