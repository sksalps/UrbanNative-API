using UrbanNative.Application.DTOs.Address;
using UrbanNative.Application.DTOs.CommonCrossDashboard;
using UrbanNative.Application.Interfaces.CommonCrossDashboard;

namespace UrbanNative.Vendors.Services
{
    public class VendorAddressService : IAddressEngineService
    {
        private readonly HttpClient _http;

        public VendorAddressService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public Task<List<AddressListDto>> GetAddressesAsync(string type)
            => _http.GetFromJsonAsync<List<AddressListDto>>($"api/address/list?type={type}");

        public Task<AddressListDto> GetByIdAsync(int id)
            => _http.GetFromJsonAsync<AddressListDto>($"api/address/{id}");

        public async Task<int> SaveAsync(AddressSaveDto dto)
        {
            var res = await _http.PostAsJsonAsync("api/address/save", dto);
            return await res.Content.ReadFromJsonAsync<int>();
        }
    }
}
