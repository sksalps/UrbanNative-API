using UrbanNative.Application.DTOs.AdminWallet;
namespace UrbanNative.Admin.Services
{
    public interface IAdminWalletService
    {
        Task<IEnumerable<WalletLedgerDto>> GetLedgerAsync(
    string ownerType,
    int ownerId,
    string walletType,
    string accHead,
    string txnType,
    string sourceType,
    int? sourceId,
    DateTime? from,
    DateTime? to);

        Task<WalletBalanceSummaryDto> GetBalanceSummaryAsync(
        string ownerType,
        int ownerId,
        string walletType,
        string accHead,
        string txnType,
        string sourceType,
        int? sourceId,
        DateTime? from,
        DateTime? to);
    }


}
