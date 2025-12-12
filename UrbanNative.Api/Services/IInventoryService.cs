using UrbanNative.Domain.Entities;

namespace UrbanNative.Api.Services
{
    public interface IInventoryService
    {
        Task<int> StockInAsync(int variantSetId, int quantity, int createdBy);
        Task<int> StockOutAsync(int variantSetId, int quantity, int createdBy);
        Task<IEnumerable<InventoryLog>> GetLogsAsync(int variantSetId);
        Task<LowStockResult?> CheckLowStockAsync(int variantSetId);
    }
}