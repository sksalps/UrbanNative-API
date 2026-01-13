using System;
using System.Text.Json;
using UrbanNative.Application.DTOs.AdminWallet;
using static System.Net.WebRequestMethods;
namespace UrbanNative.Admin.Services
{
    public class AdminWalletService : IAdminWalletService
    {
        private readonly HttpClient _http;

        public AdminWalletService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }



        public async Task<IEnumerable<WalletLedgerDto>> GetLedgerAsync(
    string ownerType,
    int ownerId,
    string walletType,
    string accHead,
    string txnType,
    string sourceType,
    int? sourceId,
    DateTime? from,
    DateTime? to)
        {
            var q = new List<string>
    {
        $"ownerType={ownerType}",
        $"ownerId={ownerId}"
    };

            if (!string.IsNullOrEmpty(walletType)) q.Add($"walletType={walletType}");
            if (!string.IsNullOrEmpty(accHead)) q.Add($"accHead={accHead}");
            if (!string.IsNullOrEmpty(txnType)) q.Add($"txnType={txnType}");
            if (!string.IsNullOrEmpty(sourceType)) q.Add($"sourceType={sourceType}");
            if (sourceId.HasValue) q.Add($"sourceId={sourceId.Value}");
            if (from.HasValue) q.Add($"from={from.Value:yyyy-MM-dd}");
            if (to.HasValue) q.Add($"to={to.Value:yyyy-MM-dd}");

            var url = "api/admin/wallet/ledger?" + string.Join("&", q);

            var json = await _http.GetStringAsync(url);

            return JsonSerializer.Deserialize<IEnumerable<WalletLedgerDto>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<WalletBalanceSummaryDto> GetBalanceSummaryAsync(
    string ownerType,
    int ownerId,
    string walletType,
    string accHead,
    string txnType,
    string sourceType,
    int? sourceId,
    DateTime? from,
    DateTime? to)
        {
            var query = new List<string>
    {
        $"ownerType={ownerType}",
        $"ownerId={ownerId}"
    };

            if (!string.IsNullOrEmpty(walletType)) query.Add($"walletType={walletType}");
            if (!string.IsNullOrEmpty(accHead)) query.Add($"accHead={accHead}");
            if (!string.IsNullOrEmpty(txnType)) query.Add($"txnType={txnType}");
            if (!string.IsNullOrEmpty(sourceType)) query.Add($"sourceType={sourceType}");
            if (sourceId.HasValue) query.Add($"sourceId={sourceId}");
            if (from.HasValue) query.Add($"from={from:yyyy-MM-dd}");
            if (to.HasValue) query.Add($"to={to:yyyy-MM-dd}");

            var url = "api/admin/wallet/balance-summary?" + string.Join("&", query);

            return await _http.GetFromJsonAsync<WalletBalanceSummaryDto>(url);
        }

    }

}
