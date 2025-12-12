
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Admin.Services;

namespace UrbanNative.Admin.Pages.Dashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IAdminService _adminService;
        public IndexModel(IAdminService adminService) { _adminService = adminService; }

        public int VendorsCount { get; set; }
        public int PendingProducts { get; set; }
        public int LowStockCount { get; set; }
        public int UsersCount { get; set; }

        public List<string> ChartLabels { get; set; } = new();
        public List<int> ChartData { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Replace with real service calls, using your APIs
            VendorsCount = await _adminService.GetVendorsCountAsync();
            PendingProducts = await _adminService.GetPendingProductsCountAsync();
            LowStockCount = await _adminService.GetLowStockCountAsync();
            UsersCount = await _adminService.GetUsersCountAsync();

            // Example chart data (last 7 days)
            ChartLabels = new List<string> { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            ChartData = new List<int> { 5, 9, 7, 10, 12, 6, 8 };
        }
    }
}