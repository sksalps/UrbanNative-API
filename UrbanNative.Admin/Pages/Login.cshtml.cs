using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using UrbanNative.Admin.Services;
using UrbanNative.Application.DTOs;

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
        public string Identifier { get; set; } = string.Empty; // username or email

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Identifier) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Enter identifier (email or username) and password.";
                return Page();
            }

            var admin = await _adminService.ValidateAdminAsync(Identifier.Trim(), Password);
            if (admin == null)
            {
                ErrorMessage = "Invalid credentials.";
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.Username ?? admin.Email ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                // optionally add roles or other claims returned by API
                new Claim(ClaimTypes.Role, "Admin")
            };

            var claimsIdentity = new ClaimsIdentity(claims, "AdminCookie");

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync("AdminCookie", new ClaimsPrincipal(claimsIdentity), authProperties);

            // Redirect to dashboard
            return RedirectToPage("/Index");
        }
    }
}