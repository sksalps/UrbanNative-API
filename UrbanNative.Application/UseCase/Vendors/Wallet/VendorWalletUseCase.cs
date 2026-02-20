using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Vendors.Wallet;
using UrbanNative.Application.Interfaces.UseCases.Wallet;
using UrbanNative.Application.Interfaces.Vendors.Wallet;

namespace UrbanNative.Application.UseCase.Vendors.Wallet
{
    public class VendorWalletUseCase : IVendorWalletUseCase
    {
        private readonly IVendorWalletRepository _repo;

        public VendorWalletUseCase(IVendorWalletRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<WalletLedgerRowDto>> ExecuteAsync(WalletLedgerFilterDto filter)
            => await _repo.GetWalletLedgerAsync(filter);

        public async Task<IEnumerable<AccountHeadDto>> GetAccountHeadsAsync(int VendorId)
            => await _repo.GetAccountHeadsAsync(VendorId);
        public async Task<WalletLedgerSummaryDto> ExecuteAsync(int vendorId, DateTime fromDate, DateTime toDate)
        => await _repo.GetLedgerSummaryAsync(vendorId, fromDate, toDate);
        public async Task<List<string>> SearchOrdersAsync(int vendorId, string term)
        {
            return (await _repo.SearchOrdersAsync(vendorId, term)).ToList();
        }

    }

}
