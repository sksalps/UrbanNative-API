using UrbanNative.Application.DTOs;

namespace UrbanNative.Admin.Services
{
    public interface IAdminProductService
    {
        // Products list (Admin)
        Task<IEnumerable<AdminProductDto>> GetProductsAsync(
            string? search,
            string? approvalStatus,
            bool? isActive
        );

        // Single product view (Admin)
        Task<AdminProductDto?> GetByIdAsync(int productId);

        // Admin actions
        Task ApproveAsync(int productId, string? remark);
        Task RejectAsync(int productId, string reason);
        Task ToggleActiveAsync(int productId);
    }
}