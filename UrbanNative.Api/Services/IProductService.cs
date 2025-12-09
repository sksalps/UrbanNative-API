using UrbanNative.Domain.Entities;

namespace UrbanNative.Api.Services
{
    public interface IProductService
    {
        Task<int> AddProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> ApproveProductAsync(int productId, int adminId);
        Task<bool> RejectProductAsync(int productId, string reason);
        Task<bool> AddProductImageAsync(int productId, string imageUrl, bool isPrimary);
        Task<Product?> GetProductByIdAsync(int productId);
        Task<IEnumerable<Product>> GetApprovedProductsAsync();
        Task<IEnumerable<Product>> GetPendingProductsAsync();
        Task<IEnumerable<Product>> GetProductsByVendorAsync(int vendorId);
    }
}