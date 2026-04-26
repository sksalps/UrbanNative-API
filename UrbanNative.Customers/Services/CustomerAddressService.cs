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

        public Task<AddressListDto> GetByIdAsync(int id, string? entityType, int? entityId)
            => _http.GetFromJsonAsync<AddressListDto>($"api/address/{id}?EntityType={entityType}&EntityId={entityId}");

        public Task<List<AddressListDto>> GetAddressListAsync()
        {
            return  _http.GetFromJsonAsync<List<AddressListDto>>($"api/address/listall");
        }

        public async Task<ServiceResultDto> DeleteAddressAsync(int addressId)
        {
            var res = await _http.PostAsJsonAsync("api/address/delete", addressId);
            var content = await res.Content.ReadAsStringAsync();
            if (!res.IsSuccessStatusCode)
            {
                return new ServiceResultDto { IsSuccess = false, Message = content };
            }

            return new ServiceResultDto { IsSuccess = true, Message = content };
        }

        public async Task<ServiceResultDto> SetPrimaryAddressAsync(int addressId)
        {
            var res = await _http.PostAsJsonAsync(
                "api/address/set-primary-address", addressId     // ✅ this sends proper JSON
            );

            var content = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
            {
                return new ServiceResultDto
                {
                    IsSuccess = false,
                    Message = content
                };
            }

            return new ServiceResultDto { IsSuccess = true };
        }


        public async Task<ServiceResultDto> SaveAddressAsync(AddressSaveDto dto)
        {
            if (dto == null)
            {
                return new ServiceResultDto
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
            //var response = await _http.PostAsJsonAsync("api/address/save", dto);

            // 🔥 Handle failure first
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();

                return new ServiceResultDto
                {
                    IsSuccess = false,
                    Message = $"API Error: {response.StatusCode} - {errorContent}"
                };
            }

            // 🔥 Read response properly (ONLY once)
            var data = await response.Content.ReadFromJsonAsync<ServiceResultDto>();

            if (data == null)
            {
                return new ServiceResultDto
                {
                    IsSuccess = false,
                    Message = "Empty response from API"
                };
            }

            // 🔥 Return actual API result (don't override blindly)
            return (new ServiceResultDto { IsSuccess = true,Message="Address Saved Successfully", AddressId= data.AddressId, EntityId=data.EntityId });
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
