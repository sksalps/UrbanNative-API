using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces.Vendors;
using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories.Vendors
{
    public class VendorWarehouseRepository : IVendorWarehouseRepository
    {

        private readonly SqlConnectionFactory _connectionFactory;

        public VendorWarehouseRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<VendorWarehouseListDto>> GetWarehousesListAsync(int vendorId)
        {
            using var conn = _connectionFactory.CreateConnection();
            var param = new { VendorID = vendorId };

            return (await conn.QueryAsync<VendorWarehouseListDto>(
                "sp_VendorWarehouses_List", param, commandType: CommandType.StoredProcedure)).ToList();
        }

        public async Task<VendorWarehouseSaveDto> GetWarehouseByIdAsync(int warehouseId, int entityId, string entityType)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<VendorWarehouseSaveDto>(
                "sp_WarehouseDetails_GetById",
                new { WarehouseId = warehouseId, EntityType = entityType, EntityID = entityId },
                commandType: CommandType.StoredProcedure
            );
        }

        // ================= SAVE Warehouse =================
        public async Task SaveWarehouseAsync(VendorWarehouseSaveDto dto)
        {
            using var conn = _connectionFactory.CreateConnection();
            var param = new
            {
                dto.WarehouseId,
                dto.WarehouseName,
                dto.ContactPerson,
                dto.Mobile,
                dto.AddressID,
                dto.IsPrimary,
                dto.EntityType,
                EntityID = dto.EntityId,
                UpdatedBy = dto.EntityType,
                UpdatedByID = dto.EntityId
            };
            await conn.ExecuteAsync("sp_Warehouse_Save", param);
        }
        public async Task DeleteWarehouseAsync(int warehouseId, int entityId)
        {
            using var conn = _connectionFactory.CreateConnection();

            await conn.ExecuteAsync(
                "sp_VendorWarehouse_Delete",
                new
                {
                    WarehouseId = warehouseId,
                    EntityType = "VENDOR",
                    VendorID = entityId
                },
                commandType: CommandType.StoredProcedure
            );
        }

    }
}
