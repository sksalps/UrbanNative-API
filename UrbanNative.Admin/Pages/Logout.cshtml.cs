using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using UrbanNative.Admin.Models;
using UrbanNative.Admin.Services;

namespace UrbanNative.Admin.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAdminService _adminService;
        public LoginModel(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [BindProperty]
        public string Identifier { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        public void OnGet(string returnUrl = null)
        {
            // optional: clear existing auth if any
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(Identifier) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Enter identifier and password.";
                return Page();
            }

            var admin = await _adminService.ValidateAdminAsync(Identifier.Trim(), Password);
            if (admin == null)
            {
                ErrorMessage = "Invalid credentials.";
                return Page();
            }

            // build claims: prefer DisplayName for Name; include role & id
            var nameClaimValue = !string.IsNullOrEmpty(admin.DisplayName)
                ? admin.DisplayName
                : (!string.IsNullOrEmpty(admin.Username) ? admin.Username : admin.Email);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, nameClaimValue),
                new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                new Claim("display_name", admin.DisplayName ?? string.Empty),
                new Claim(ClaimTypes.Email, admin.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, admin.Role ?? "Admin")
            };

            var claimsIdentity = new ClaimsIdentity(claims, "AdminCookie");
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync("AdminCookie", new ClaimsPrincipal(claimsIdentity), authProperties);

            // redirect to returnUrl if safe, else to Index/Dashboard
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToPage("/Index"); // or your dashboard page
        }
    }
}