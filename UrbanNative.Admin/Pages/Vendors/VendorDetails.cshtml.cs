using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.AdminVendor;
using UrbanNative.Admin.Services;

namespace UrbanNative.Admin.Pages.Vendors
{
    public class VendorDetailsModel : PageModel
    {
        private readonly IAdminVendorService _vendorService;

        public VendorDetailsModel(IAdminVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        public AdminVendorDetailDto Vendor { get; set; } = null!;
        public VendorReferralInfoDto? ReferralInfo { get; set; }
        public IEnumerable<VendorMediaDto> Media { get; set; }
    = Enumerable.Empty<VendorMediaDto>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var vendor = await _vendorService.GetByIdAsync(id);
            ReferralInfo = await _vendorService.GetVendorReferralAsync(id);
            Media = await _vendorService.GetVendorMediaAsync(id);

            if (vendor == null)
                return NotFound();

            Vendor = vendor;
            return Page();
        }

        
        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            await _vendorService.UpdateApprovalAsync(id, "Approved", null);
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostRejectAsync(int id, string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                ModelState.AddModelError("", "Reason is required");
                return Page();
            }

            await _vendorService.UpdateApprovalAsync(id, "Rejected", reason);
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostToggleActiveAsync(int id)
        {
            await _vendorService.ToggleActiveAsync(id);
            return RedirectToPage(new { id });
        }

    }
}