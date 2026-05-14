using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using UrbanNative.Application.DTOs.Customers.AuthLogin;
using UrbanNative.Customers.Services;
using UrbanNative.Domain.Entities;

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

        public async Task<IActionResult> OnPostSendOtpAsync([FromBody] SendOtpRequestDto input)
        {
            if (input == null)
                throw new Exception("Input is null");
            try
            {
                var result = await _service.SendOtpAsync(input.Identifier, input.ReferralCode);

                return new JsonResult(new
                {
                    success = true,
                    message = "OTP_SENT",
                    otp = result.OTP,
                    expiryAt = result.ExpiryAt,
                    byReferralCode = input.ReferralCode
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
                var result = await _service.VerifyOtpAsync(input.Identifier, input.OTP, input.ByReferralCode);

                await setclaims(result);

                return new JsonResult(result);
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
        private async Task setclaims(VerifyOtpResponseDto result)
        {
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, result.userExist.UserID.ToString()), // standard
                    new Claim("UserID", result.userExist.UserID.ToString()),
                    new Claim(ClaimTypes.Name, result.userExist.FullName ?? ""),
                    new Claim("UserNickName", result.userExist.UserNickName ?? ""),
                    new Claim(ClaimTypes.Role, "Customer"), // important for policy
                    new Claim("JWT", result.userExist.Token)   // 🔑 THIS is what JwtTokenHandler reads
                };

                var identity = new ClaimsIdentity(claims, "CustomerCookie");

                await HttpContext.SignInAsync("CustomerCookie", new ClaimsPrincipal(identity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                    });
            }            

        }
    }
}