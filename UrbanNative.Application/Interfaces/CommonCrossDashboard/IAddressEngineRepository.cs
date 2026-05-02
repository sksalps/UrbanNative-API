using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Address;
using UrbanNative.Application.DTOs.CommonCrossDashboard;

namespace UrbanNative.Application.Interfaces.CommonCrossDashboard
{
    public interface IAddressEngineRepository
    {

        Task<List<AddressListDto>> GetListAsync(string entityType, int entityId, string addressType);
        Task<List<AddressListDto>> GetListAllAsync(string entityType, int entityId);
        Task SetPrimaryAddressAsync(int addressId, int entityId, string entityType);
        Task DeleteAddressAsync(int addressId, int entityId, string entityType);
        Task<AddressListDto> GetByIdAsync(int addressId, string entityType, int entityId);
        Task<AddressSaveResultDto> SaveAsync(AddressSaveDto dto);

        Task<IEnumerable<CountryDto>> GetCountriesAsync(int? countryId);
        Task<IEnumerable<StateDto>> GetStatesAsync(int? countryId, int? stateId);
        Task<IEnumerable<CityDto>> GetCitiesAsync(int? stateId, int? cityId);
    }
    
}
