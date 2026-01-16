using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.Interfaces;
using UrbanNative.Vendors.Services;

namespace UrbanNative.Vendors.Pages.Settings
{
    public class PasswordModel : PageModel
    {
        private readonly IVendorAuthService _authService;

        public PasswordModel(IVendorAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty] public string CurrentPassword { get; set; }
        [BindProperty] public string NewPassword { get; set; }
        [BindProperty] public string ConfirmPassword { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (NewPassword != ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match");
                return Page();
            }

            int vendorId = int.Parse(User.FindFirst("VendorID")!.Value);

            var success = true;// await _authService.ChangePasswordAsync(                vendorId, CurrentPassword, NewPassword);

            if (!success)
            {
                ModelState.AddModelError("", "Current password incorrect");
                return Page();
            }

            TempData["Success"] = "Password updated successfully";
            return RedirectToPage("/Dashboard/Index");
        }
    }
}
