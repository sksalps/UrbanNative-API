using Dapper;
using System.Data;
using UrbanNative.Application.DTOs.AdminCategory;
using UrbanNative.Application.DTOs.AdminVariantSet;
using UrbanNative.Application.Interfaces;
using UrbanNative.Infrastructure.Database;


namespace UrbanNative.Infrastructure.Repositories
{

    public class AdminVariantSetRepository : IAdminVariantSetRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public AdminVariantSetRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        public async Task<IEnumerable<AdminVariantSetListDto>> GetAllAsync()
        {
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryAsync<AdminVariantSetListDto>(
                "sp_AdminVariantSets_GetAll",
                commandType: CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<AdminCategoryListDto>> GetAvailableCategoriesAsync(int variantSetId)
        {
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryAsync<AdminCategoryListDto>(
                "sp_AdminVariantSetCategories_GetAvailable",
                new { VariantSetID = variantSetId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<AdminVariantSetDetailsDto?> GetByIdAsync(int variantSetId)
        {
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<AdminVariantSetDetailsDto>(
                "sp_AdminVariantSets_GetById",
                new { VariantSetID = variantSetId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task CreateAsync(string variantSetName)
        {
            using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(
                "sp_AdminVariantSets_Create",
                new { VariantSetName = variantSetName },
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateAsync(int variantSetId, string variantSetName)
        {
            using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(
                "sp_AdminVariantSets_Update",
                new { VariantSetID = variantSetId, VariantSetName = variantSetName },
                commandType: CommandType.StoredProcedure);
        }

        public async Task ToggleStatusAsync(int variantSetId)
        {
            using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(
                "sp_AdminVariantSets_ToggleStatus",
                new { VariantSetID = variantSetId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<AdminVariantInsideSetDto>> GetVariantsBySetIdAsync(int variantSetId)
        {
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryAsync<AdminVariantInsideSetDto>(
                "sp_AdminVariantSetVariants_GetBySetId",
                new { VariantSetID = variantSetId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task AddVariantToSetAsync(int variantSetId, int variantId)
        {
            using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(
                "sp_AdminVariantSetVariants_Add",
                new { VariantSetID = variantSetId, VariantID = variantId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task RemoveVariantFromSetAsync(int variantSetVariantId)
        {
            using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(
                "sp_AdminVariantSetVariants_Remove",
                new { VariantSetVariantID = variantSetVariantId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateVariantOrderAsync(int variantSetVariantId, int newSortOrder)
        {
            using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(
                "sp_AdminVariantSetVariants_UpdateOrder",
                new { VariantSetVariantID = variantSetVariantId, NewSortOrder = newSortOrder },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<AdminVariantSetCategoryDto>> GetCategoriesBySetIdAsync(int variantSetId)
        {
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryAsync<AdminVariantSetCategoryDto>(
                "sp_AdminVariantSetCategories_GetBySetId",
                new { VariantSetID = variantSetId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task AssignCategoryAsync(int variantSetId, int categoryId)
        {
            using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(
                "sp_AdminVariantSetCategories_Assign",
                new { VariantSetID = variantSetId, CategoryID = categoryId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task RemoveCategoryAsync(int categoryVariantSetId)
        {
            using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(
                "sp_AdminVariantSetCategories_Remove",
                new { CategoryVariantSetID = categoryVariantSetId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task MoveVariantAsync(int variantSetVariantId, string direction)
        {
            using var conn = _connectionFactory.CreateConnection();

            await conn.ExecuteAsync(
                "sp_AdminVariantInsideSet_Move",
                new
                {
                    VariantSetVariantID = variantSetVariantId,
                    Direction = direction
                },
                commandType: CommandType.StoredProcedure);
        }

    }

}