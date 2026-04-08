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
            Task<AddressListDto> GetByIdAsync(int addressId, string entityType, int entityId);
            Task<int> SaveAsync(AddressSaveDto dto, string entityType, int entityId);
    }
    
}
