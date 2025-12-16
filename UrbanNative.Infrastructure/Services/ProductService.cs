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

        // =========================
        // Admin – Actions
        // =========================
        public async Task<bool> ApproveProductAsync(int productId, int adminId)
        {
            return await _productRepository.ApproveProductAsync(productId, adminId);
        }

        public async Task<bool> RejectProductAsync(int productId, string reason)
        {
            return await _productRepository.RejectProductAsync(productId, reason);
        }

        public async Task<bool> ToggleActiveAsync(int productId)
        {
            return await _productRepository.ToggleActiveAsync(productId);
        }
    }
}
