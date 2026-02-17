using UrbanNative.Application.DTOs.Vendors.Wallet;


    public interface IVendorWalletService
    {
        Task<List<WalletLedgerRowDto>> GetLedgerAsync(WalletLedgerFilterDto filter);
        Task<List<AccountHeadDto>> GetAccountHeadsAsync();
    }

