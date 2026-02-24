using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.UseCase.Vendors.Wallet;
using UrbanNative.Application.DTOs.Vendors.Wallet;

namespace UrbanNative.Application.Interfaces.UseCases.Wallet
{
    public interface ISettlementPayoutUseCase
    {
        Task<SettlementPayoutSummaryDto> GetSummaryAsync(int vendorId, string filterType, DateTime? fromDate, DateTime? toDate);
        Task<IEnumerable<SettlementPayoutDto>> GetSettlementsAsync(int vendorId, string filterType, DateTime? fromDate, DateTime? toDate);
        Task<IEnumerable<SettlementPaymentDto>> GetPaymentsAsync(long settlementId, int vendorId);
    }
}
