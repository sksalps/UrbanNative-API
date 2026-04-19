using System.Net.Http;
using System.Net.Http.Json;
using UrbanNative.Application.DTOs.Customers.AuthLogin;
using UrbanNative.Customers.Services.Interfaces;
using static System.Net.WebRequestMethods;

namespace UrbanNative.Customers.Services
{
    public class CustomerAuthService : ICustomerAuthService
    {
        private readonly HttpClient _http;

        public CustomerAuthService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public async Task<SendOtpResponseDto> SendOtpAsync(string identifier)
        {
            var response = await _http.PostAsJsonAsync(
                "api/customer/auth/send-otp",
                new { Identifier = identifier }
            );

            return await response.Content.ReadFromJsonAsync<SendOtpResponseDto>();
        }

        public async Task<VerifyOtpResponseDto> VerifyOtpAsync(string identifier, int otp)
        {
            var res = await _http.PostAsJsonAsync(
                "api/customer/auth/verify-otp",
                new { Identifier = identifier, OTP = otp });

            return await res.Content.ReadFromJsonAsync<VerifyOtpResponseDto>();
        }
    }

    
}