using UrbanNative.Application.DTOs.Vendors.Orders;
using UrbanNative.Application.DTOs.Vendors.Wallet;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Services
{
    public class VendorWalletService: IVendorWalletService
    {
        private readonly HttpClient _http;

        public VendorWalletService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public async Task<List<WalletLedgerRowDto>> GetLedgerAsync(WalletLedgerFilterDto filter)
        {
            var response = await _http.PostAsJsonAsync($"api/vendors/wallet/ledger", filter);

            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadFromJsonAsync<List<WalletLedgerRowDto>>()
                ?? new();
        }
        public async Task<List<AccountHeadDto>> GetAccountHeadsAsync()
        {
            return await _http.GetFromJsonAsync<List<AccountHeadDto>>($"api/vendors/wallet/account-heads");
        }
        public async Task<WalletLedgerSummaryDto> GetLedgerSummaryAsync(WalletLedgerFilterDto filter)
        {
            var fromDate = filter.FromDate ?? DateTime.Today.AddDays(-30);
            var toDate = filter.ToDate ?? DateTime.Today;

            var url = $"api/vendors/wallet/ledgersummary?fromDate={fromDate:yyyy-MM-dd}&toDate={toDate:yyyy-MM-dd}";

            var result = await _http.GetFromJsonAsync<WalletLedgerSummaryDto>(url);

            return result ?? new WalletLedgerSummaryDto();
        }

        
        public async Task<List<WalletSearchSuggestionDto>> SmartSearchAsync(string term)
        {
            var url = $"api/vendors/wallet/smart-search?term={Uri.EscapeDataString(term)}";
            return await _http.GetFromJsonAsync<List<WalletSearchSuggestionDto>>(url);
        }
        public async Task<VendorWalletSummaryReportDto> GetWalletSummaryAsync(WalletSummaryFilterDto filter)
        {
            var url = $"api/vendors/wallet/summary-report?fromDate={filter.FromDate:yyyy-MM-dd}&toDate={filter.ToDate:yyyy-MM-dd}&walletTypeId={filter.WalletTypeId}";
            return await _http.GetFromJsonAsync<VendorWalletSummaryReportDto>(url);
        }

        public async Task<List<WalletTypeDto>> GetWalletTypesAsync()
        {
            return await _http.GetFromJsonAsync<List<WalletTypeDto>>($"api/vendors/wallet/wallet-types");
        }
        public async Task<byte[]> ExportSummaryPdfAsync(WalletSummaryFilterDto filter)
        {
            var from = filter.FromDate ?? DateTime.Today.AddDays(-30);
            var to = filter.ToDate ?? DateTime.Today;

            var url = $"api/vendors/wallet/summary-report/pdf?fromDate={from:yyyy-MM-dd}&toDate={to:yyyy-MM-dd}&walletTypeId={filter.WalletTypeId}";

            return await _http.GetByteArrayAsync(url);
        }
        public async Task<byte[]> ExportSummaryExcelAsync(WalletSummaryFilterDto filter)
        {
            var from = filter.FromDate ?? DateTime.Today.AddDays(-30);
            var to = filter.ToDate ?? DateTime.Today;

            var url = $"api/vendors/wallet/summary-report/excel?fromDate={from:yyyy-MM-dd}&toDate={to:yyyy-MM-dd}&walletTypeId={filter.WalletTypeId}";

            return await _http.GetByteArrayAsync(url);
        }


    }

}
