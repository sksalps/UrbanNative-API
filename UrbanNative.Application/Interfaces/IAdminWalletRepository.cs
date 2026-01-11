using  UrbanNative.Application.DTOs.AdminWallet;

namespace UrbanNative.Application.Interfaces
{
    using UrbanNative.Application.DTOs.AdminWallet;

    public interface IAdminWalletRepository
    {
        Task<IEnumerable<WalletLedgerDto>> GetLedgerAsync(
            string ownerType,
            int ownerId,
            string? walletType,
            string? accHead,
            string? txnType,
            string? sourceType,
            int? sourceId,
            DateTime? from,
            DateTime? to
        );
    }


}
