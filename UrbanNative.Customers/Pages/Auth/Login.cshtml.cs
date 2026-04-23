using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using UrbanNative.Application.DTOs.Customers.AuthLogin;
using UrbanNative.Customers.Services;

namespace UrbanNative.Customers.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly CustomerAuthService _service;

        public LoginModel(CustomerAuthService service)
        {
            _service = service;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; }

        public class LoginInputModel
        {
            public string Identifier { get; set; }
            public int OTP { get; set; }

            public string LoginId { get; set; }
            public string Password { get; set; }
        }
        /*
        // STEP 1: Send OTP
        public async Task<IActionResult> OnPostSendOtpAsync()
        {
            await _service.SendOtpAsync(Input.Identifier);

            TempData["msg"] = "OTP Sent (check DB for now)";
            return Page();
        }

        // STEP 2: Verify OTP
        public async Task<IActionResult> OnPostVerifyOtpAsync()
        {
            var result = await _service.VerifyOtpAsync(Input.Identifier, Input.OTP);

            if (result.Status == "SUCCESS")
            {
                // Store JWT
                Response.Cookies.Append("CustomerAuthToken", result.Token,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTime.UtcNow.AddDays(30)
                    });

                // Cookie auth (Razor protection)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, result.UserID.ToString()),
                    new Claim(ClaimTypes.Role, "Customer")
                };

                var identity = new ClaimsIdentity(claims, "CustomerCookie");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("CustomerCookie", principal);

                return RedirectToPage("/Dashboard/Index");
            }

            if (result.Status == "NEW_USER")
            {
                return RedirectToPage("/Auth/Register",
                    new { tempId = result.TempID, token = result.TempToken });
            }

            TempData["error"] = "Invalid OTP";
            return Page();
        }


        */

        public async Task<IActionResult> OnPostSendOtpAsync([FromBody] SendOtpRequestDto input)
        {
            if (input == null)
                throw new Exception("Input is null");
            try
            {
                var result = await _service.SendOtpAsync(input.Identifier);

                return new JsonResult(new
                {
                    success = true,
                    message = "OTP_SENT",
                    otp = result.OTP,
                    expiryAt = result.ExpiryAt
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        public async Task<IActionResult> OnPostVerifyOtpAsync([FromBody] VerifyOtpRequestDto input)
        {
            try
            {
                var result = await _service.VerifyOtpAsync(input.Identifier, input.OTP);

                //return new JsonResult(result);
                return new JsonResult(new
                {
                    result
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}