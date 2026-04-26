using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Address;
using UrbanNative.Application.DTOs.CommonCrossDashboard;

namespace UrbanNative.Application.Interfaces.CommonCrossDashboard
{
    public interface IAddressEngineService
    {
        Task<List<AddressListDto>> GetAddressesLookupAsync(string type, int? entityId = null);
        Task<AddressListDto> GetByIdAsync(int id, string? type, int? entityId);
        Task<ServiceResultDto> SaveAddressAsync(AddressSaveDto dto);
        Task<List<AddressListDto>> GetAddressListAsync();
        Task<ServiceResultDto> DeleteAddressAsync(int addressId);
        Task<ServiceResultDto> SetPrimaryAddressAsync(int addressId);

        Task<List<CountryDto>> GetCountriesAsync(int? countryId);
        Task<List<StateDto>> GetStatesAsync(int? countryId, int? stateId);
        Task<List<CityDto>> GetCitiesAsync(int? stateId, int? cityId);
    }

}
