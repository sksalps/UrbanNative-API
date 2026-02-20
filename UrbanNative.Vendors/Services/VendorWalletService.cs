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

        public async Task<List<string>> SearchOrdersAsync(string term)
        {
            var url = $"api/vendors/wallet/order-search?term={Uri.EscapeDataString(term)}";
            return await _http.GetFromJsonAsync<List<string>>(url);
        }


    }

}
