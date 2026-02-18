using UrbanNative.Application.DTOs.Vendors.Wallet;

namespace UrbanNative.Vendors.Services.Interfaces
{

    public interface IVendorWalletService
    {
        Task<List<WalletLedgerRowDto>> GetLedgerAsync(WalletLedgerFilterDto filter);
        Task<List<AccountHeadDto>> GetAccountHeadsAsync();
    }

}