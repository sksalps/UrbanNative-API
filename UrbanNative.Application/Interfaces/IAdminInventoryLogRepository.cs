using UrbanNative.Application.DTOs.AdminInventory;

namespace UrbanNative.Application.Interfaces
{
    public interface IAdminInventoryLogRepository
    {
        Task<AdminInventoryLogPeriodResultDto> GetLogsBySkuPeriodAsync(
            int skuId,
            DateTime fromDate,
            DateTime toDate);
    }
}
