using UrbanNative.Application.DTOs;
using UrbanNative.Domain.Entities;

//This is using on Admin Dashboard for product management and product listing "IAdminProductRepository"

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
        //Get product details by id for admin
        Task<AdminProductDto?> GetAdminProductByIdAsync(int productId);
            //Get product images by product id for admin
        Task<IEnumerable<ProductImageDto>> GetProductImagesAsync(int productId);

        // =========================
        // Admin – Actions
        // =========================
        Task<bool> ApproveProductAsync(int productId, int adminId, string remark);
        Task<bool> RejectProductAsync(int productId, int adminId, string reason);

        Task<bool> ToggleActiveAsync(int productId);
    }
}
