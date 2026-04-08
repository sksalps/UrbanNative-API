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
        Task<AddressListDto> GetByIdAsync(int addressId, string entityType, int entityId);
        Task<int> SaveAsync(AddressSaveDto dto, int entityId, string entityType);
    }
}
