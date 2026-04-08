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
        Task<List<AddressListDto>> GetAddressesAsync(string type);
        Task<AddressListDto> GetByIdAsync(int id);
        Task<int> SaveAsync(AddressSaveDto dto);
    }

}
