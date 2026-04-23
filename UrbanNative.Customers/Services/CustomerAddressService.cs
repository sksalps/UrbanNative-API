using System.Text.Json;
using UrbanNative.Application.DTOs.CommonCrossDashboard;
using UrbanNative.Application.Interfaces.CommonCrossDashboard;

namespace UrbanNative.Customers.Services
{
    public class CustomerAddressService : IAddressEngineService
    {
        private readonly HttpClient _http;

        public CustomerAddressService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public Task<List<AddressListDto>> GetAddressesLookupAsync(string type, int? entityId = null)
            => _http.GetFromJsonAsync<List<AddressListDto>>($"api/address/list?type={type}&entityId={entityId}");

        public Task<AddressListDto> GetByIdAsync(int id)
            => _http.GetFromJsonAsync<AddressListDto>($"api/address/{id}");

        public Task<List<AddressListDto>> GetAddressListAsync()
        {
            return  _http.GetFromJsonAsync<List<AddressListDto>>($"api/address/listall");
        }

        public async Task<ServiceResult> DeleteAddressAsync(int addressId)
        {
            var res = await _http.PostAsJsonAsync("api/address/delete", addressId);
            var content = await res.Content.ReadAsStringAsync();
            if (!res.IsSuccessStatusCode)
            {
                return new ServiceResult { IsSuccess = false, Message = content };
            }

            return new ServiceResult { IsSuccess = true, Message = content };
        }

        public async Task<ServiceResult> SetPrimaryAddressAsync(int addressId)
        {
            var res = await _http.PostAsJsonAsync(
                "api/address/set-primary-address", addressId     // ✅ this sends proper JSON
            );

            var content = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
            {
                return new ServiceResult
                {
                    IsSuccess = false,
                    Message = content
                };
            }

            return new ServiceResult { IsSuccess = true };
        }


        public async Task<ServiceResult> SaveAddressAsync(AddressSaveDto dto)
        {
            if (dto == null)
            {
                return new ServiceResult
                {
                    IsSuccess = false,
                    Message = "Address data is required."
                };
            }
            HttpResponseMessage response;

            if (dto.EntityType == "NATIVE")   // 🔥 fix NATIVE → TEMP
            {
                // Anonymous call
                response = await _http.PostAsJsonAsync("api/address/savenative", dto);
            }
            else
            {
                // JWT call
                response = await _http.PostAsJsonAsync("api/address/save", dto);
            }
            //var result = await response.Content.ReadFromJsonAsync<AddressSaveResponseDto>();
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return new ServiceResult
                {
                    IsSuccess = false,
                    Message = $"API Error: {response.StatusCode} - {content}"
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
