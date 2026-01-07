using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Admin.Services;
using UrbanNative.Application.DTOs.AdminOrders;

namespace UrbanNative.Admin.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly IAdminOrderService _orderService;

        public IndexModel(IAdminOrderService orderService)
        {
            _orderService = orderService;
        }


        public List<AdminOrderListDto> Orders { get; set; } = new();

        // Optional filters (Phase-1 safe)
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string OrderStatus { get; set; }

        public async Task OnGetAsync(
            DateTime? fromDate,
            DateTime? toDate,
            string orderStatus
        )
        {
            FromDate = fromDate;
            ToDate = toDate;
            OrderStatus = orderStatus;

            Orders = await _orderService.GetOrdersAsync(
                fromDate,
                toDate,
                orderStatus
            );
        }
    }
}
