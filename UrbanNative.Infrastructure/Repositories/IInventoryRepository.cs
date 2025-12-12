using UrbanNative.Domain.Entities;

namespace UrbanNative.Infrastructure.Repositories
{
    public interface IInventoryRepository
    {
        Task<int> StockInAsync(int variantSetId, int quantity, int createdBy);
        Task<int> StockOutAsync(int variantSetId, int quantity, int createdBy);
        Task<IEnumerable<InventoryLog>> GetInventoryLogsAsync(int variantSetId);
        Task<LowStockResult?> CheckLowStockAsync(int variantSetId);
    }
}