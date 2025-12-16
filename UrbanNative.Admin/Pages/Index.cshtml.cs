using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.Interfaces;

public class DashboardModel : PageModel
{
    private readonly IAdminService _adminService;

    public DashboardModel(IAdminService adminService)
    {
        _adminService = adminService;
    }

    public int VendorsCount { get; set; }
    public int PendingProductsCount { get; set; }
    public int LowStockCount { get; set; }
    public int UsersCount { get; set; }

    public async Task OnGet()
    {
        VendorsCount = await _adminService.GetVendorsCountAsync();
        PendingProductsCount = await _adminService.GetPendingProductsCountAsync();
        LowStockCount = await _adminService.GetLowStockCountAsync();
        UsersCount = await _adminService.GetUsersCountAsync();
    }
}
