using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Repositories;

namespace UrbanNative.Api.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _repo;

        public InventoryService(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public Task<int> StockInAsync(int variantSetId, int quantity, int createdBy)
            => _repo.StockInAsync(variantSetId, quantity, createdBy);

        public Task<int> StockOutAsync(int variantSetId, int quantity, int createdBy)
            => _repo.StockOutAsync(variantSetId, quantity, createdBy);

        public Task<IEnumerable<InventoryLog>> GetLogsAsync(int variantSetId)
            => _repo.GetInventoryLogsAsync(variantSetId);

        public Task<LowStockResult?> CheckLowStockAsync(int variantSetId)
            => _repo.CheckLowStockAsync(variantSetId);
    }
}