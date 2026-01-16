using System.Net.Http.Json;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces;

namespace UrbanNative.Vendors.Services
{
    public class VendorAuthService : IVendorAuthService
    {
        private readonly HttpClient _http;

        public VendorAuthService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public async Task<VendorLoginResponseDto?> LoginAsync(VendorLoginRequestDto request)
        {
            var response = await _http.PostAsJsonAsync("/api/vendor/auth/login", request);

            if (!response.IsSuccessStatusCode)
                return null;

            //return await response.Content.ReadFromJsonAsync<VendorLoginResponseDto>();

            var data = await response.Content.ReadFromJsonAsync<VendorLoginResponseDto>();

            //Console.WriteLine("VENDOR LOGIN RESPONSE:");
            //Console.WriteLine(data?.Token);

            return data;

        }
    }
}
