using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Vendors.Services;

namespace UrbanNative.Vendors.Pages.Dashboard
{
    [Authorize(Policy = "VendorOnly")]

    public class IndexModel : PageModel
    {
        private readonly IVendorDashboardService _dashboardService;

        public VendorDashboardDto Stats { get; private set; }

        public IndexModel(IVendorDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task OnGetAsync()
        {
            Stats = await _dashboardService.GetDashboardAsync();
        }
    }
}
