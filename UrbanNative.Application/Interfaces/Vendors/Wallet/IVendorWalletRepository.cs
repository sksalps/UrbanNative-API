using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Vendors.Wallet;

namespace UrbanNative.Application.Interfaces.Vendors.Wallet
{
    public interface IVendorWalletRepository
    {
        Task<IEnumerable<WalletLedgerRowDto>> GetWalletLedgerAsync(WalletLedgerFilterDto filter);
        Task<IEnumerable<AccountHeadDto>> GetAccountHeadsAsync(int VendorID);
        Task<WalletLedgerSummaryDto> GetLedgerSummaryAsync(int vendorId, DateTime fromDate, DateTime toDate);
        //Task<IEnumerable<string>> SearchOrdersAsync(int vendorId, string term);
        Task<IEnumerable<WalletSearchSuggestionDto>> SmartSearchAsync(int vendorId, string term);
        Task<VendorWalletSummaryReportDto> GetWalletSummaryAsync(int vendorId, DateTime from, DateTime to, int? walletTypeId);
        Task<List<VendorWalletTypeDto>> GetWalletTypesAsync(int vendorId);
        Task<VendorContextDto> GetVendorContextAsync(int vendorId);
    }
}


