using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Address;
using UrbanNative.Application.DTOs.CommonCrossDashboard;

namespace UrbanNative.Application.Interfaces.UseCases.CommonCrossDashboard
{
    public interface IAddressEngineUseCase
    {
        Task<List<AddressListDto>> GetListAsync(int entityId, string entityType, string type);
        Task<List<AddressListDto>> GetListAllAsync(int entityId, string entityType);
        Task DeleteAddressAsync(int addressId, int entityId, string entityType);
        Task ExecuteSetPrimaryAsync(int addressId, int entityId, string entityType);
        Task<AddressListDto> GetByIdAsync(int addressId, string entityType, int entityId);
        Task<int> SaveAsync(AddressSaveDto dto, int entityId, string entityType);

        Task<IEnumerable<CountryDto>> GetCountriesAsync(int? countryId);
        Task<IEnumerable<StateDto>> GetStatesAsync(int? countryId, int? stateId);
        Task<IEnumerable<CityDto>> GetCitiesAsync(int? stateId, int? cityId);
    }
}
