
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Vendors.Inventory;
using UrbanNative.Application.Interfaces.Vendors;
using UrbanNative.Application.Interfaces.UseCases;

namespace UrbanNative.Application.UseCase
{
    public class VendorInventoryUseCase : IVendorInventoryUseCase
    {
        private readonly IVendorInventoryRepository _repository;

        public VendorInventoryUseCase(IVendorInventoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<VendorInventorySummaryDto> GetInventorySummaryAsync(
        int skuId,
        int addressId,
        DateTime fromDate,
        DateTime toDate)
        {
            return await _repository.GetInventorySummaryAsync(
                skuId, addressId, fromDate, toDate);
        }


        public Task<IReadOnlyList<VendorInventoryLogDto>> GetInventoryLogsAsync(
            int skuId,
            int addressId,
            DateTime fromDate,
            DateTime toDate,
            int page,
            int pageSize)
        {
            return _repository.GetInventoryLogsAsync(
                skuId, addressId, fromDate, toDate, page, pageSize);
        }
    }

}
