using System.Net.Http.Json;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Services
{
    public class VendorProfileService : IVendorProfileService
    {
        
        private readonly HttpClient _http;

        public VendorProfileService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public async Task<VendorProfileDto> GetProfileAsync()
        {
            var data= await _http.GetFromJsonAsync<VendorProfileDto>($"/api/vendor/profile");
            return data;
        }

        public async Task UpdateProfileAsync(VendorProfileDto dto)
        {
            var response = await _http.PutAsJsonAsync($"/api/vendor/profile", dto);

            response.EnsureSuccessStatusCode();
        }

        
    }
}
