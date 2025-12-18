using UrbanNative.Application.DTOs;
using UrbanNative.Application.Interfaces;
using UrbanNative.Domain.Entities;


namespace UrbanNative.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // =========================
        // Vendor / Public
        // =========================
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _productRepository.GetProductsAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _productRepository.GetProductByIdAsync(productId);
        }


        // =========================
        // Admin – Products Listing
        // =========================
        public async Task<IEnumerable<AdminProductDto>> GetAdminProductsAsync(
            string? search,
            string? approvalStatus,
            bool? isActive)
        {
            return await _productRepository.GetAdminProductsAsync(
                search,
                approvalStatus,
                isActive
            );
        }
        //Get product details by id for admin
        
        public async Task<AdminProductDto?> GetAdminProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetAdminProductByIdAsync(productId);
            if (product == null) return null;

            var images = await _productRepository.GetProductImagesAsync(productId);
            product.Images = images.ToList();

            return product;
        }

        // =========================
        // Admin – Actions
        // =========================
        public async Task<bool> ApproveProductAsync(int productId, int adminId, string? remark)
        {
            remark ??= "Approved";
            return await _productRepository.ApproveProductAsync(productId, adminId, remark);
        }

        public async Task<bool> RejectProductAsync(int productId,          int adminId,           string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Reject reason is required.");

            return await _productRepository.RejectProductAsync(productId, adminId, reason);
        }

        public async Task<bool> ToggleActiveAsync(int productId)
        {
            return await _productRepository.ToggleActiveAsync(productId);
        }
    }
}
