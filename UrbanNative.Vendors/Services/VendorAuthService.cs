using System.Net.Http.Json;
using UrbanNative.Application.DTOs.Common;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces;

namespace UrbanNative.Vendors.Services
{
    public class VendorAuthService : IVendorAuthService
    {
        private readonly HttpClient _http;
        private readonly object _httpContextAccessor;

        public VendorAuthService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public async Task<VendorLoginResponseDto?> LoginAsync(VendorLoginRequestDto request)
        {
            var response = await _http.PostAsJsonAsync("/api/vendor/auth/login", request);
            if (!response.IsSuccessStatusCode)
                return null;
            var data = await response.Content.ReadFromJsonAsync<VendorLoginResponseDto>();
            //Console.WriteLine("VENDOR LOGIN RESPONSE:");
            //Console.WriteLine(data?.Token);
            return data;
        }


        public async Task<bool> ChangePasswordAsync(ChangePasswordRequestDto request)
        {
            /*var requestMsg = new HttpRequestMessage(HttpMethod.Post, "/api/vendor/account/change-password")
            {
                Content = JsonContent.Create(request)
            };

            var ctx = _httpContextAccessor.HttpContext;
            if (ctx != null && ctx.Request.Headers.TryGetValue("Cookie", out var cookie))
            {
                // Forward browser cookies so the API can authenticate the vendor
                requestMsg.Headers.TryAddWithoutValidation("Cookie", (string)cookie);
            }*/
            var response = await _http.PostAsJsonAsync("/api/vendor/account/change-password",request);
            //return true;

            return response.IsSuccessStatusCode;
        }



    }
}
