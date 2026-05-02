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
                    byReferralCode=input.ReferralCode
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
                var result = await _service.VerifyOtpAsync(input.Identifier, input.OTP,input.ByReferralCode);

                return new JsonResult( result );



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