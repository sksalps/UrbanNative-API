using UrbanNative.Application.DTOs;
using UrbanNative.Domain.Entities;

namespace UrbanNative.Application.Interfaces
{
    public interface IProductService
    {
        // =========================
        // Vendor / Public APIs
        // =========================
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product?> GetProductByIdAsync(int productId);

        // =========================
        // Admin – Products Listing
        // =========================
        Task<IEnumerable<AdminProductDto>> GetAdminProductsAsync(
            string? search,
            string? approvalStatus,
            bool? isActive
        );

        Task<AdminProductDto?> GetAdminProductByIdAsync(int productId);
        public interface IAdminProductService
        {
            Task<AdminProductDto?> GetByIdAsync(int productId);
        }
        // =========================
        // Admin – Actions
        // =========================
        Task<bool> ApproveProductAsync(int productId, int adminId, string? remark);
        Task<bool> RejectProductAsync(int productId, int adminId, string reason);
        Task<bool> ToggleActiveAsync(int productId);
    }
}

