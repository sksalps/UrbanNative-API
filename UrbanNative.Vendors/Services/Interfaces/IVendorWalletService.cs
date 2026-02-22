using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.DTOs.Vendors.Wallet;

namespace UrbanNative.Vendors.Services.Interfaces
{

    public interface IVendorWalletService
    {
        Task<List<WalletLedgerRowDto>> GetLedgerAsync(WalletLedgerFilterDto filter);
        Task<List<AccountHeadDto>> GetAccountHeadsAsync();
        Task<WalletLedgerSummaryDto> GetLedgerSummaryAsync(WalletLedgerFilterDto filter);
        Task<List<WalletSearchSuggestionDto>> SmartSearchAsync(string term);
        Task<VendorWalletSummaryReportDto> GetWalletSummaryAsync(WalletSummaryFilterDto filter);
        Task<List<VendorWalletTypeDto>> GetWalletTypesAsync();
        Task<byte[]> ExportSummaryPdfAsync(WalletSummaryFilterDto filter);
        Task<byte[]> ExportSummaryExcelAsync(WalletSummaryFilterDto filter);
    }

}