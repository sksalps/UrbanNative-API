using UrbanNative.Application.DTOs.AdminInventory;

namespace UrbanNative.Admin.Services
{
    public interface IAdminInventoryLogService
    {
        Task<AdminInventoryLogPeriodResultDto?> GetSkuLogsByPeriodAsync(
            int skuId,
            DateTime fromDate,
            DateTime toDate
        );
    }
}
