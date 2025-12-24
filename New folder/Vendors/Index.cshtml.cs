using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.AdminVendor;
using UrbanNative.Admin.Services;

namespace UrbanNative.Admin.Pages.Vendors
{
    public class IndexModel : PageModel
    {
        private readonly IAdminVendorService _vendorService;

        public IndexModel(IAdminVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        public IEnumerable<AdminVendorListDto> Vendors { get; set; } = [];

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ApprovalStatus { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? IsActive { get; set; }

        public async Task OnGetAsync()
        {
            Vendors = await _vendorService.GetVendorsAsync(
                Search,
                ApprovalStatus,
                IsActive
            );
        }
    }
}