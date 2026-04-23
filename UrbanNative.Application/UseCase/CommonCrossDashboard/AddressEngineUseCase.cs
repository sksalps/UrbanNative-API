using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Address;
using UrbanNative.Application.DTOs.CommonCrossDashboard;
using UrbanNative.Application.Interfaces.CommonCrossDashboard;
using UrbanNative.Application.Interfaces.UseCases.CommonCrossDashboard;

namespace UrbanNative.Application.UseCase.CommonCrossDashboard
{
    public class AddressEngineUseCase : IAddressEngineUseCase
    {
        private readonly IAddressEngineRepository _repo;

        public AddressEngineUseCase(IAddressEngineRepository repo)
        {
            _repo = repo;
        }

        public Task<List<AddressListDto>> GetListAsync(int entityId,string entityType, string type)
            => _repo.GetListAsync(entityType, entityId, type);
        public Task<List<AddressListDto>> GetListAllAsync(int entityId, string entityType)
           => _repo.GetListAllAsync(entityType, entityId);
        public async Task DeleteAddressAsync(int addressId, int entityId, string entityType)
           =>  _repo.DeleteAddressAsync(addressId, entityId, entityType);
        

        public async Task ExecuteSetPrimaryAsync(int addressId, int entityId, string entityType)
            => _repo.SetPrimaryAddressAsync(addressId, entityId, entityType);
        


        public Task<AddressListDto> GetByIdAsync(int id, string entityType, int entityId)
            => _repo.GetByIdAsync(id,entityType,entityId);

        public Task<int> SaveAsync(AddressSaveDto dto)
            => _repo.SaveAsync(dto);

        public async Task<IEnumerable<CountryDto>> GetCountriesAsync(int? countryId)
            =>     await _repo.GetCountriesAsync(countryId);
        

        public async Task<IEnumerable<StateDto>> GetStatesAsync(int? countryId, int? stateId)
        {
            return await _repo.GetStatesAsync(countryId, stateId);
        }

        public async Task<IEnumerable<CityDto>> GetCitiesAsync(int? stateId, int? cityId)
        {
            return await _repo.GetCitiesAsync(stateId, cityId);
        }
    }
}
