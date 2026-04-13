using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Application.DTOs.Vendors;

namespace UrbanNative.Application.Interfaces.Vendors
{
    public interface IVendorWarehouseRepository
    {
        Task<List<VendorWarehouseListDto>> GetWarehousesListAsync(int vendorId);
        Task DeleteWarehouseAsync(int warehouseId, int entityId);
        Task SetPrimaryWarehouseAsync(int warehouseId, int entityId, string entityType);
        Task<VendorWarehouseSaveDto> GetWarehouseByIdAsync(int warehouseId, int entityId, string entityType);
        Task SaveWarehouseAsync(VendorWarehouseSaveDto dto, int EntityId, string EntityType);
    }
}
