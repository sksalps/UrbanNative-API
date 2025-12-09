using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Repositories;

namespace UrbanNative.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public Task<int> AddProductAsync(Product product)
            => _repo.AddProductAsync(product);

        public Task<bool> UpdateProductAsync(Product product)
            => _repo.UpdateProductAsync(product);

        public Task<bool> ApproveProductAsync(int productId, int adminId)
            => _repo.ApproveProductAsync(productId, adminId);

        public Task<bool> RejectProductAsync(int productId, string reason)
            => _repo.RejectProductAsync(productId, reason);

        public Task<bool> AddProductImageAsync(int productId, string imageUrl, bool isPrimary)
            => _repo.AddProductImageAsync(productId, imageUrl, isPrimary);

        public Task<Product?> GetProductByIdAsync(int productId)
            => _repo.GetProductByIdAsync(productId);

        public Task<IEnumerable<Product>> GetApprovedProductsAsync()
            => _repo.GetApprovedProductsAsync();

        public Task<IEnumerable<Product>> GetPendingProductsAsync()
            => _repo.GetPendingProductsAsync();

        public Task<IEnumerable<Product>> GetProductsByVendorAsync(int vendorId)
            => _repo.GetProductsByVendorAsync(vendorId);
    }
}
