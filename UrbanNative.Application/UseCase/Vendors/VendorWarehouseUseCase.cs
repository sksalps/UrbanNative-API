using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces.UseCases;
using UrbanNative.Application.Interfaces.Vendors;


namespace UrbanNative.Application.UseCase.Vendors
{
    public class VendorWarehouseUseCase : IVendorWarehouseUseCase
    {
        private readonly IVendorWarehouseRepository _repository;

        public VendorWarehouseUseCase(IVendorWarehouseRepository repository)
        {
            _repository = repository;
        }
        public Task<List<VendorWarehouseListDto>> GetWarehousesListAsync( int vendorId)
            => _repository.GetWarehousesListAsync(vendorId);

        public async Task DeleteWarehouseAsync(int warehouseId, int EntityId)
        {
            await _repository.DeleteWarehouseAsync(warehouseId, EntityId);
        }

        public async Task ExecuteSetPrimaryAsync(int warehouseId, int EntityId, string EntityType)
        {
            await _repository.SetPrimaryWarehouseAsync(warehouseId, EntityId, EntityType);
        }
        public async Task<VendorWarehouseSaveDto> GetWarehouseByIdAsync(int warehouseId, int EntityId, string EntityType)
        {
            return await _repository.GetWarehouseByIdAsync(warehouseId, EntityId, EntityType);
        }
        // ================= SAVE Warehouse =================
        public async Task SaveWarehouseAsync(VendorWarehouseSaveDto dto, int EntityId, string EntityType)
        {
            await _repository.SaveWarehouseAsync(dto, EntityId, EntityType);
        }

    }
}
