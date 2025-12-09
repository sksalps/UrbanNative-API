using UrbanNative.Domain.Entities;

namespace UrbanNative.Infrastructure.Repositories
{
    public interface IProductRepository
    {
        Task<int> AddProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> ApproveProductAsync(int productId, int approvedBy);
        Task<bool> RejectProductAsync(int productId, string reason);
        Task<bool> AddProductImageAsync(int productId, string imageUrl, bool isPrimary);
        Task<Product?> GetProductByIdAsync(int productId);
        Task<IEnumerable<Product>> GetProductsByVendorAsync(int vendorId);
        Task<IEnumerable<Product>> GetPendingProductsAsync();
        Task<IEnumerable<Product>> GetApprovedProductsAsync();
    }
}