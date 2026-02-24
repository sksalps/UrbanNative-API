// Services/SettlementPayoutService.cs
using UrbanNative.Application.DTOs.Vendors.Wallet;
using UrbanNative.Vendors.Services.Interfaces;
namespace UrbanNative.Vendors.Services
{
    public class VendorSettlementPayoutService: IVendorSettlementPayoutService
    {
        private readonly HttpClient _http;

        public VendorSettlementPayoutService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public async Task<SettlementPayoutSummaryDto> GetSummaryAsync()
        => await _http.GetFromJsonAsync<SettlementPayoutSummaryDto>("api/vendors/payout/summary");

        public async Task<List<SettlementPayoutDto>> GetSettlementsAsync()
            => await _http.GetFromJsonAsync<List<SettlementPayoutDto>>("api/vendors/payout/settlements");

        public async Task<List<SettlementPaymentDto>> GetPaymentsAsync(long settlementId)
            => await _http.GetFromJsonAsync<List<SettlementPaymentDto>>($"api/vendors/payout/payments?settlementId={settlementId}");
    }
}