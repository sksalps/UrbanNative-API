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
            //var query = QueryStringHelper.ToQueryString(filter);
            return await _http.GetFromJsonAsync<List<WalletLedgerRowDto>>(
                $"api/vendors/wallet/ledger{filter}");
            //await _http.PostAsJsonAsync("api/vendor/logistics/shipments", dto)
        }
        

        public async Task<List<AccountHeadDto>> GetAccountHeadsAsync()
        {
            return await _http.GetFromJsonAsync<List<AccountHeadDto>>("api/vendors/wallet/account-heads");
        }
    }

}
