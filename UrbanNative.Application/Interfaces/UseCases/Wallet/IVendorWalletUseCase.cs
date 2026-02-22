using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Vendors.Wallet;

namespace UrbanNative.Application.Interfaces.UseCases.Wallet
{
    public interface IVendorWalletUseCase
    {
        Task<IEnumerable<WalletLedgerRowDto>> ExecuteAsync(WalletLedgerFilterDto filter);
        Task<IEnumerable<AccountHeadDto>> GetAccountHeadsAsync(int VendorId);
        Task<WalletLedgerSummaryDto> ExecuteAsync(int vendorId, DateTime fromDate, DateTime toDate);
        Task<List<WalletSearchSuggestionDto>> SmartSearchAsync(int vendorId, string term);
        Task<VendorWalletSummaryReportDto> ExecuteSummaryAsync(int vendorId, DateTime from, DateTime to, int? walletTypeId);
        Task<List<VendorWalletTypeDto>> ExecuteWalletTypeAsync(int vendorId);
    }

}
