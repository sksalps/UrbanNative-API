using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Vendors.Wallet;

namespace UrbanNative.Application.Interfaces.Vendors.Wallet
{
    public interface ISettlementPayoutRepository
    {
        Task<SettlementPayoutSummaryDto> GetSummaryAsync(string entityType, int entityId, string filterType, DateTime? fromDate, DateTime? toDate);

        Task<IEnumerable<SettlementPayoutDto>> GetSettlementsAsync(string entityType, int entityId, string filterType, DateTime? fromDate, DateTime? toDate);

        Task<IEnumerable<SettlementPaymentDto>> GetPaymentsAsync(long settlementId, string entityType, int entityId);
    }
}
