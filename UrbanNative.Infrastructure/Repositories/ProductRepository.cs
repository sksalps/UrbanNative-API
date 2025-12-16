using Dapper;
using UrbanNative.Application.DTOs;
using UrbanNative.Application.Interfaces;
using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public ProductRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // =========================
        // Vendor / Public
        // =========================
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Product>(
                "sp_Public_GetProducts",
                commandType: System.Data.CommandType.StoredProcedure
            );
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Product>(
                "sp_Public_GetProductById",
                new { ProductID = productId },
                commandType: System.Data.CommandType.StoredProcedure
            );
        }

        // =========================
        // Admin – Products Listing
        // =========================
        public async Task<IEnumerable<AdminProductDto>> GetAdminProductsAsync(
            string? search,
            string? approvalStatus,
            bool? isActive)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryAsync<AdminProductDto>(
                "sp_Admin_GetProducts",
                new
                {
                    Search = search,
                    ApprovalStatus = approvalStatus,
                    IsActive = isActive
                },
                commandType: System.Data.CommandType.StoredProcedure
            );
        }

        // =========================
        // Admin – Actions
        // =========================
        public async Task<bool> ApproveProductAsync(int productId, int adminId)
        {
            using var connection = _connectionFactory.CreateConnection();

            var rows = await connection.ExecuteAsync(
                "sp_Admin_ApproveProduct",
                new { ProductID = productId, AdminID = adminId },
                commandType: System.Data.CommandType.StoredProcedure
            );

            return rows > 0;
        }

        public async Task<bool> RejectProductAsync(int productId, string reason)
        {
            using var connection = _connectionFactory.CreateConnection();

            var rows = await connection.ExecuteAsync(
                "sp_Admin_RejectProduct",
                new { ProductID = productId, Reason = reason },
                commandType: System.Data.CommandType.StoredProcedure
            );

            return rows > 0;
        }

        public async Task<bool> ToggleActiveAsync(int productId)
        {
            using var connection = _connectionFactory.CreateConnection();

            var rows = await connection.ExecuteAsync(
                "sp_Admin_ToggleProductActive",
                new { ProductID = productId },
                commandType: System.Data.CommandType.StoredProcedure
            );

            return rows > 0;
        }
    }
}
