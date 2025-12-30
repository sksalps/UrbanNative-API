using Dapper;
using System.Data;
using UrbanNative.Application.DTOs.AdminCategory;
using UrbanNative.Application.Interfaces;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    public class AdminCategoryHSNRepository : IAdminCategoryHSNRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public AdminCategoryHSNRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<CategoryHSNDto?> GetByCategoryIdAsync(int categoryId)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<CategoryHSNDto>(
                "sp_CategoryHSN_GetByCategoryId",
                new { CategoryId = categoryId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task LinkOrUpdateAsync(CategoryHSNLinkDto dto)
        {
            using var conn = _connectionFactory.CreateConnection();

            await conn.ExecuteAsync(
                "sp_CategoryHSN_LinkOrUpdate",
                new
                {
                    dto.CategoryId,
                    dto.HSNId,
                    AdminId = dto.AdminId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task RemoveAsync(int categoryId, int adminId)
        {
            using var conn = _connectionFactory.CreateConnection();

            await conn.ExecuteAsync(
                "sp_CategoryHSN_Remove",
                new
                {
                    CategoryId = categoryId,
                    AdminId = adminId
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}