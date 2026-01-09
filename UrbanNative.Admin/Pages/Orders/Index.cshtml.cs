using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Admin.Models;
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
        public OrderFilterModel Filter { get; set; } = new();

        public int TotalRecords { get; set; }
        public int TotalPages =>
            (int)Math.Ceiling((double)TotalRecords / Filter.PageSize);

        public async Task OnGetAsync(OrderFilterModel filter)
        {
            Filter = filter;

            var result = await _orderService.GetOrdersAsync(filter);

            Orders = result.Orders;
            TotalRecords = result.TotalRecords;
        }
    }
}
