using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.Customers;
using UrbanNative.Application.DTOs.Customers.AuthLogin;
using UrbanNative.Customers.Services;

namespace UrbanNative.Customers.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly CustomerAuthService _service;

        public RegisterModel(CustomerAuthService service)
        {
            _service = service;
        }

        // ==========================
        // 🔹 Bind Properties (Prefill)
        // ==========================
        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Mobile { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public int? ReferredByUserID { get; set; }
        [BindProperty]
        public string Pincode { get; set; }

        [BindProperty]
        public string TempToken { get; set; }

        [BindProperty]
        public int? TempID { get; set; }

        // ==========================
        // 🔹 LOAD PAGE
        // ==========================
        public async Task<IActionResult> OnGetAsync(int? tempId, string token)
        {
            TempID = tempId;
            TempToken = token;

            // 🔍 If temp exists → load prefill
            if (tempId.HasValue && !string.IsNullOrEmpty(token))
            {
                var temp = await _service.GetTempUserAsync(tempId.Value, token);

                if (temp != null)
                {
                    Mobile = temp.Mobile;
                    Email = temp.Email;
                    Name = temp.FullName;
                    ReferredByUserID = temp.ReferredByUserID;
                }
            }

            return Page();
        }
        public async Task<IActionResult> OnPostInsertAsync([FromBody] CreateTempUserRequestDto dto)
        {
            try
            {
                // 🔥 enrich server-side
                dto.UserAgent = Request.Headers["User-Agent"].ToString();
                dto.IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

                var result = await _service.InsertTempUserAsync(dto);

                if (result == null)
                    return new JsonResult(new { success = false });

                return new JsonResult(new
                {
                    success = true,
                    tempID = result.TempID,
                    tempToken = result.TempToken
                });
            }
            catch
            {
                return new JsonResult(new { success = false });
            }
        }

        // ==========================
        // 🔹 REGISTER (POST) complete registration after OTP verification
        // ==========================

        public async Task<IActionResult> OnPostCompleteAsync()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            var input = System.Text.Json.JsonSerializer.Deserialize<RegisterRequestDto>(body);

            if (input == null)
            {
                return new JsonResult(new { Status = "ERROR", Message = "Invalid request" });
            }

            var result = await _service.RegisterAsync(input);

            return new JsonResult(result);
        }
    }
}