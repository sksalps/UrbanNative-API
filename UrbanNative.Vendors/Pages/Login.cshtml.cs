using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Numerics;
using System.Security.Claims;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces;
using UrbanNative.Domain.Entities;
using UrbanNative.Vendors.Services;

namespace UrbanNative.Vendors.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IVendorAuthService _auth;

        public LoginModel(IVendorAuthService auth)
        {
            _auth = auth;
        }

        [BindProperty]
        public string Identifier { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string Error { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _auth.LoginAsync(new VendorLoginRequestDto
            {
                Identifier = Identifier,
                Password = Password
            });

            if (result == null)
            {
                Error = "Invalid credentials or vendor not approved.";
                return Page();
            }

            // Create cookie identity



            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, result.VendorID.ToString()), // standard
                new Claim("VendorID", result.VendorID.ToString()),
                new Claim(ClaimTypes.Role, "Vendor"),
                new Claim("JWT", result.Token)   // 🔑 THIS is what JwtTokenHandler reads
            };
            

            //var identity = new ClaimsIdentity(claims, "VendorCookie");
            // var principal = new ClaimsPrincipal(identity);

            //await HttpContext.SignInAsync("VendorCookie", principal);
            //Console.Write (result.VendorID);

            // return RedirectToPage("/Dashboard/Index");


            var identity = new ClaimsIdentity(claims, "VendorCookie");

            await HttpContext.SignInAsync("VendorCookie",   new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                });



            //HttpContext.Session.SetInt32("VendorID", result.VendorID);

            return RedirectToPage("/dashboard/Index");
        }
    }
}
