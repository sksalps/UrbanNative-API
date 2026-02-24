using UrbanNative.Application.DTOs.Vendors.Wallet;
namespace UrbanNative.Vendors.Services.Interfaces
{
    public interface IVendorSettlementPayoutService
    {
        Task<SettlementPayoutSummaryDto> GetSummaryAsync();
        Task<List<SettlementPayoutDto>> GetSettlementsAsync();
        Task<List<SettlementPaymentDto>> GetPaymentsAsync(long settlementId);
    }
}