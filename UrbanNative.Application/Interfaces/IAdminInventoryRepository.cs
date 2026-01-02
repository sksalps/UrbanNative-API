using UrbanNative.Application.DTOs.AdminInventory;

namespace UrbanNative.Application.Interfaces
{
    public interface IAdminInventoryRepository
    {
        Task<IEnumerable<AdminInventoryListDto>> GetInventoryAsync(
            string? search,
            int? categoryId,
            bool? lowStockOnly,
            bool? isActive
        );

        Task<AdminInventorySummaryDto?> GetInventorySummaryAsync(int productId);

        Task<IEnumerable<AdminInventorySkuDto>> GetInventorySkusAsync(int productId);
    }
}
