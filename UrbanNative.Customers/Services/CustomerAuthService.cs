using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
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

            //return await res.Content.ReadFromJsonAsync<VerifyOtpResponseDto>();

            //var text = await res.Content.ReadAsStringAsync();


            //Console.WriteLine("RAW API RESPONSE: " + text);
            // 🔥 FIX: unwrap double-serialized JSON
            
            try
            {
                return await res.Content.ReadFromJsonAsync<VerifyOtpResponseDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("DESERIALIZATION ERROR: " + ex.Message);

                return new VerifyOtpResponseDto
                {
                    success = false,
                    Status = "ERROR"
                };
            }
        }


        public async Task<CreateTempUserResponseDto?> InsertTempUserAsync(CreateTempUserRequestDto dto)
        {
            var res = await _http.PostAsJsonAsync("api/customer/auth/insert-tempuser", dto);

            if (!res.IsSuccessStatusCode)
                return null;

            return await res.Content.ReadFromJsonAsync<CreateTempUserResponseDto>();
        }
        public async Task<TempUserDto> GetTempUserAsync(int tempId, string token)
        {
            var res = await _http.GetAsync(
                $"api/customer/auth/temp-user?tempId={tempId}&token={token}"
            );

            if (!res.IsSuccessStatusCode)
                return null;

            return await res.Content.ReadFromJsonAsync<TempUserDto>();
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto req)
        {
            var res = await _http.PostAsJsonAsync("api/customer/auth/register",req);

            return await res.Content.ReadFromJsonAsync<RegisterResponseDto>();
        }
    }

    
}