using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Vendors;

namespace UrbanNative.Application.Interfaces.UseCases
{
    public interface IVendorWarehouseUseCase
    {
        Task<List<VendorWarehouseListDto>> GetWarehousesListAsync(int vendorId);
        Task DeleteWarehouseAsync(int warehouseId, int EntityId);
        Task<VendorWarehouseSaveDto> GetWarehouseByIdAsync(int warehouseId, int EntityId, string EntityType);
        Task SaveWarehouseAsync(VendorWarehouseSaveDto dto, int EntityId, string EntityType);
    }
}
