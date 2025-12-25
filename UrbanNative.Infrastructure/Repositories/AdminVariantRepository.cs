using System.Data;
using Dapper;
using UrbanNative.Application.DTOs.AdminVariant;
using UrbanNative.Application.Interfaces;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    public class AdminVariantRepository : IAdminVariantRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;


        public AdminVariantRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task UpdateVariantValueAsync(int variantValueId, string valueName)
        {
            using var conn = _connectionFactory.CreateConnection();

            await conn.ExecuteAsync(
                "sp_AdminVariantValues_Update",
                new
                {
                    VariantValueID = variantValueId,
                    ValueName = valueName
                },
                commandType: CommandType.StoredProcedure);
        }


        public async Task<IEnumerable<AdminVariantListDto>> GetVariantsAsync(
            string? search,
            bool? isActive)
        {
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryAsync<AdminVariantListDto>(
                "sp_AdminVariants_GetAll",
                new { Search = search, IsActive = isActive },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<AdminVariantDetailsDto?> GetVariantByIdAsync(int variantId)
        {
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<AdminVariantDetailsDto>(
                "sp_AdminVariants_GetById",
                new { VariantID = variantId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<AdminVariantValueDto>> GetVariantValuesAsync(int variantId)
        {
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryAsync<AdminVariantValueDto>(
                "sp_AdminVariantValues_GetByVariantId",
                new { VariantID = variantId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task CreateVariantAsync(CreateVariantDto dto)
        {
            using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(
                "sp_AdminVariants_Create",
                new { dto.VariantName },
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateVariantAsync(int variantId, string variantName)
        {
            using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(
                "sp_AdminVariants_Update",
                new { VariantID = variantId, VariantName = variantName },
                commandType: CommandType.StoredProcedure);
        }

        public async Task ToggleVariantStatusAsync(int variantId)
        {
            using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(
                "sp_AdminVariants_ToggleStatus",
                new { VariantID = variantId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task CreateVariantValueAsync(CreateVariantValueDto dto)
        {
            using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(
                "sp_AdminVariantValues_Create",
                new { dto.VariantID, dto.ValueName },
                commandType: CommandType.StoredProcedure);
        }

        public async Task ToggleVariantValueStatusAsync(int variantValueId)
        {
            using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(
                "sp_AdminVariantValues_ToggleStatus",
                new { VariantValueID = variantValueId },
                commandType: CommandType.StoredProcedure);
        }
    }
}
