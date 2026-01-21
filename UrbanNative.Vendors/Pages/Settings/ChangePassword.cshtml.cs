using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.Common;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces;
using UrbanNative.Vendors.Services;

namespace UrbanNative.Vendors.Pages.Settings
{
    public class ChangePasswordModel : PageModel
    {
        
        private readonly IVendorAuthService _service;

        public ChangePasswordModel(IVendorAuthService service)
        {
            _service = service;
        }
        [BindProperty]
        public string OldPassword { get; set; } = string.Empty;

        [BindProperty]
        public string NewPassword { get; set; } = string.Empty;

        [BindProperty]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (NewPassword != ConfirmPassword)
            {
                ErrorMessage = "New password and confirm password do not match.";
                return Page();
            }

            var success = await _service.ChangePasswordAsync( new ChangePasswordRequestDto
                {
                    OldPassword = OldPassword,
                    NewPassword = NewPassword
                });

            if (!success)
            {
                ErrorMessage = "Current password is incorrect.";
                return Page();
            }

            TempData["SuccessMessage"] = "Password changed successfully. Please login again.";
            await HttpContext.SignOutAsync();
            // Optional: auto logout redirect
            return RedirectToPage("/Login");

        }
    }
}
