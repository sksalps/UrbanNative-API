using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using UrbanNative.Application.Interfaces;

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

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Identifier) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Enter identifier (email or username) and password.";
                return Page();
            }

            // 🔐 JWT-based login
            var result = await _adminService.ValidateAdminAsync(
                Identifier.Trim(),
                Password);

            if (result == null)
            {
                ErrorMessage = "Invalid credentials";
                return Page();
            }

            // ==============================
            // 1️⃣ Store JWT for API calls
            // ==============================
            Response.Cookies.Append(
                "jwt",
                result.Token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, // true in production
                    SameSite = SameSiteMode.Lax
                });

            // ==============================
            // 2️⃣ Sign in Razor UI (Cookie)
            // ==============================
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, result.Admin.Id.ToString()),
                new Claim(ClaimTypes.Name, result.Admin.Username ?? result.Admin.Email ?? ""),
                new Claim(ClaimTypes.Role, result.Admin.Role ?? "Admin")
            };

            var identity = new ClaimsIdentity(claims, "AdminCookie");

            await HttpContext.SignInAsync(
                "AdminCookie",
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                });

            //HttpContext.Session.SetInt32("AdminId", result.Admin.Id);

            return RedirectToPage("/Index");
        }
    }
}
