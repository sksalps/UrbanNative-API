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
        Task<List<AddressListDto>> GetAddressesLookupAsync(string type);
        Task<AddressListDto> GetByIdAsync(int id);
        Task<ServiceResult> SaveAddressAsync(AddressSaveDto dto);
        Task<List<AddressListDto>> GetAddressListAsync();
        Task<ServiceResult> DeleteAddressAsync(int addressId);
        Task<ServiceResult> SetPrimaryAddressAsync(int addressId);

        Task<List<CountryDto>> GetCountriesAsync(int? countryId);
        Task<List<StateDto>> GetStatesAsync(int? countryId, int? stateId);
        Task<List<CityDto>> GetCitiesAsync(int? stateId, int? cityId);
    }

}
