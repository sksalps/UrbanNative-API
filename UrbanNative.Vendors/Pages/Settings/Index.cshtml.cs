using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Domain.Entities;

namespace UrbanNative.Vendors.Pages.Settings
{
    public class IndexModel : PageModel
    {
        private readonly IVendorSettingsService _service;

        public IndexModel(IVendorSettingsService service)
        {
            _service = service;
        }

        [BindProperty]
        public VendorSettingsDto Options { get; set; }

        public async Task OnGetAsync()
        {
            int vendorId = int.Parse(User.FindFirst("VendorID")!.Value);
            Options = await _service.GetSettingsAsync(vendorId);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            int vendorId = int.Parse(User.FindFirst("VendorID")!.Value);
            await _service.SaveSettingsAsync(vendorId, Options);

            TempData["Success"] = "Settings saved";
            return RedirectToPage();
        }
    }
}
