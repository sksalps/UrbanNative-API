using System.Data;
using Dapper;
using UrbanNative.Application.DTOs.AdminHSN;
using UrbanNative.Application.Interfaces;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    public class AdminHSNRepository : IAdminHSNRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public AdminHSNRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> CreateAsync(AdminHSNCreateDto dto)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.ExecuteScalarAsync<int>(
                "sp_HSN_Insert",
                new
                {
                    dto.HSNCode,
                    dto.Description,
                    dto.GSTId,
                    dto.CreatedBy
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateAsync(AdminHSNUpdateDto dto)
        {
            using var conn = _connectionFactory.CreateConnection();

            await conn.ExecuteAsync(
                "sp_HSN_Update",
                new
                {
                    dto.HSNId,
                    dto.HSNCode,
                    dto.Description,
                    dto.GSTId,
                    dto.UpdatedBy
                },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<IEnumerable<AdminHSNListDto>> GetFilterAsync(string? search, int? gstId,  bool? isActive)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryAsync<AdminHSNListDto>(
                "sp_AdminHSN_GetAll",
                new
                {
                    Search = search,
                    GSTId = gstId,
                    IsActive = isActive
                },
                commandType: CommandType.StoredProcedure
            );
        }

     

        public async Task<AdminHSNDetailDto?> GetByIdAsync(int hsnId)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<AdminHSNDetailDto>(
                "sp_HSN_GetById",
                new { HSNId = hsnId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task ToggleActiveAsync(int hsnId, int updatedBy)
        {
            using var conn = _connectionFactory.CreateConnection();

            await conn.ExecuteAsync(
                "sp_HSN_ToggleActive",
                new
                {
                    HSNId = hsnId,
                    UpdatedBy = updatedBy
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}