using UrbanNative.Application.Interfaces.UseCases.Wallet;
using UrbanNative.Application.Interfaces.Vendors.Wallet;
using UrbanNative.Application.DTOs.Vendors.Wallet;

namespace UrbanNative.Application.UseCase;

public class SettlementPayoutUseCase : ISettlementPayoutUseCase
{
    private readonly ISettlementPayoutRepository _repo;

    public SettlementPayoutUseCase(ISettlementPayoutRepository repo)
    {
        _repo = repo;
    }

    public Task<SettlementPayoutSummaryDto> GetSummaryAsync(int vendorId, string filterType, DateTime? fromDate, DateTime? toDate)
        => _repo.GetSummaryAsync("Vendor", vendorId, filterType, fromDate, toDate);

    public Task<IEnumerable<SettlementPayoutDto>> GetSettlementsAsync(int vendorId, string filterType, DateTime? fromDate, DateTime? toDate)
        => _repo.GetSettlementsAsync("Vendor", vendorId, filterType, fromDate, toDate);

    public Task<IEnumerable<SettlementPaymentDto>> GetPaymentsAsync(long settlementId, int vendorId)
        => _repo.GetPaymentsAsync(settlementId, "Vendor", vendorId);
}