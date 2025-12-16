using UrbanNative.Application.DTOs;
using UrbanNative.Domain.Entities;
using UrbanNative.Application.Interfaces;

namespace UrbanNative.Application.Interfaces
{
    public interface IProductRepository
    {
        // =========================
        // Vendor / Public
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

        // =========================
        // Admin – Actions
        // =========================
        Task<bool> ApproveProductAsync(int productId, int adminId);
        Task<bool> RejectProductAsync(int productId, string reason);
        Task<bool> ToggleActiveAsync(int productId);
    }
}
