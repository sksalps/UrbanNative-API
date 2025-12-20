using Dapper;
using System.Data;
using UrbanNative.Application.DTOs;
using UrbanNative.Application.Interfaces;
using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Database;


//This is using on Admin Dashboard for product management and product listing "AdminProductRepository"
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
        //Get product details by id for admin
        public async Task<AdminProductDto?> GetAdminProductByIdAsync(int productId)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<AdminProductDto>(
                "sp_Admin_GetProductById",
                new { ProductId = productId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<ProductImageDto>> GetProductImagesAsync(int productId)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryAsync<ProductImageDto>(
                "sp_Admin_GetProductImages",
                new { ProductID = productId },
                commandType: CommandType.StoredProcedure
            );
        }

        // =========================
        // Admin – Actions
        // =========================

        // =========================================
        // APPROVE PRODUCT (WITH HISTORY)
        // =========================================
        public async Task<bool> ApproveProductAsync(int productId, int adminId, string remark)
        {
            using var conn = _connectionFactory.CreateConnection();

            await conn.ExecuteAsync(
                "sp_Admin_ApproveProduct",
                new { ProductId = productId, AdminId = adminId, Remark = remark },
                commandType: CommandType.StoredProcedure
            );

            return true;
        }



        // =========================================
        // REJECT PRODUCT (WITH HISTORY)
        // =========================================
        public async Task<bool> RejectProductAsync(int productId, int adminId, string reason)
        {
            using var conn = _connectionFactory.CreateConnection();

            await conn.ExecuteAsync(
                "sp_Admin_RejectProduct",
                new { ProductId = productId, AdminId = adminId, Reason = reason },
                commandType: CommandType.StoredProcedure
            );

            return true;
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

