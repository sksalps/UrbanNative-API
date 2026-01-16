using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Pages.Business
{
    public class VendorProfileModel : PageModel
    {
        private readonly IVendorProfileService _service;

        public VendorProfileModel(IVendorProfileService service)
        {
            _service = service;
        }

        [BindProperty]
        public VendorProfileDto Profile { get; set; }

        public async Task OnGetAsync()
        {
            Profile = await _service.GetProfileAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _service.UpdateProfileAsync(Profile);

            TempData["Success"] = "Profile updated successfully";
            return RedirectToPage();
        }
    }
}
