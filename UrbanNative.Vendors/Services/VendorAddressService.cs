using System.Text.Json;
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

        public Task<List<AddressListDto>> GetAddressesLookupAsync(string type)
            => _http.GetFromJsonAsync<List<AddressListDto>>($"api/address/list?type={type}");

        public Task<AddressListDto> GetByIdAsync(int id)
            => _http.GetFromJsonAsync<AddressListDto>($"api/address/{id}");

        public async Task<ServiceResult> SaveAddressAsync(AddressSaveDto dto)
        {
            var res = await _http.PostAsJsonAsync("api/address/save", dto);

            var content = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
            {
                return new ServiceResult
                {
                    IsSuccess = false,
                    Message = $"API Error: {res.StatusCode} - {content}"
                };
            }

            return new ServiceResult { IsSuccess = true, Message = "Address Saved Successfully", AddressId=dto.AddressID };
        }


        public async Task<List<CountryDto>> GetCountriesAsync(int? countryId)
        {
            return await _http.GetFromJsonAsync<List<CountryDto>>(
                $"api/address/country?countryId={countryId}");
        }

        public async Task<List<StateDto>> GetStatesAsync(int? countryId, int? stateId)
        {
            return await _http.GetFromJsonAsync<List<StateDto>>(
                $"api/address/state?countryId={countryId}&stateId={stateId}");
        }

        public async Task<List<CityDto>> GetCitiesAsync(int? stateId, int? cityId)
        {
            return await _http.GetFromJsonAsync<List<CityDto>>(
                $"api/address/city?stateId={stateId}&cityId={cityId}");
        }
    }
}
