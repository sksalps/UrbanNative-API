using Dapper;
using System.Data;
using UrbanNative.Application.DTOs.AdminCategory;
using UrbanNative.Application.Interfaces;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    public class AdminCategoryRepository : IAdminCategoryRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public AdminCategoryRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // =========================
        // Admin – Category Listing
        // =========================
        public async Task<IEnumerable<AdminCategoryListDto>> GetAdminCategoriesAsync()
        {
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryAsync<AdminCategoryListDto>(
                "sp_AdminCategories_GetAll",
                commandType: CommandType.StoredProcedure);
        }

        // =========================
        // Admin – Category Details
        // =========================
        public async Task<AdminCategoryDetailDto?> GetAdminCategoryByIdAsync(int categoryId)
        {
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<AdminCategoryDetailDto>(
                "sp_AdminCategories_GetById",
                new { CategoryID = categoryId },
                commandType: CommandType.StoredProcedure);
        }

        // =========================
        // Admin – Create Category
        // =========================
        public async Task CreateCategoryAsync(
            AdminCategorySaveDto dto,
            int adminUserId)
        {
            using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(
                "sp_AdminCategories_Create",
                new
                {
                    dto.ParentCategoryID,
                    dto.CategoryName,
                    dto.Description,
                    dto.SortOrder,
                    CreatedBy = adminUserId
                },
                commandType: CommandType.StoredProcedure);
        }

        // =========================
        // Admin – Update Category
        // =========================
        public async Task<bool> UpdateCategoryAsync(
            int categoryId,
            AdminCategorySaveDto dto,
            int adminUserId)
        {
            using var conn = _connectionFactory.CreateConnection();

            var rows = await conn.ExecuteAsync(
                "sp_AdminCategories_Update",
                new
                {
                    CategoryID = categoryId,
                    dto.CategoryName,
                    dto.Description,
                    dto.SortOrder,
                    UpdatedBy = adminUserId
                },
                commandType: CommandType.StoredProcedure);

            return rows > 0;
        }

        // =========================
        // Admin – Activate / Deactivate
        // =========================
        //public async Task<bool> ToggleCategoryActiveAsync(int categoryId)
        public async Task<(bool Success, string Message)> ToggleCategoryActiveAsync(int categoryId)
        {
            using var conn = _connectionFactory.CreateConnection();

            var result = await conn.QuerySingleAsync<ToggleResultDto>(
                "sp_AdminCategories_ToggleActive",
                new { CategoryID = categoryId },
                commandType: CommandType.StoredProcedure);

            return (result.Success, result.Message);
        }

    }
}
