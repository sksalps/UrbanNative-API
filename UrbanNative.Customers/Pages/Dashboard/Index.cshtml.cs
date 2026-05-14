using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.Customers;
using UrbanNative.Customers.Services.Interfaces;

namespace UrbanNative.Customers.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly ICustomerDashboardService _service;

        public CustomersDashboardDto DashboardData { get; set; }

        public IndexModel(ICustomerDashboardService service)
        {
            _service = service;
            DashboardData = new CustomersDashboardDto();
        }

        public async Task OnGetAsync()
        {
            DashboardData = await _service.GetDashboardAsync();
        }
    }
}