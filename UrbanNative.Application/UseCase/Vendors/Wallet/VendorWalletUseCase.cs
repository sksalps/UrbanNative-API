using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Vendors.Wallet;
using UrbanNative.Application.Interfaces.UseCases.Wallet;
using UrbanNative.Application.Interfaces.Vendors.Wallet;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        
        public async Task<List<WalletSearchSuggestionDto>> SmartSearchAsync(int vendorId, string term)
        {
            return (await _repo.SmartSearchAsync(vendorId, term)).ToList();
        }


        public async Task<VendorWalletSummaryReportDto> ExecuteSummaryAsync(int vendorId, DateTime from, DateTime to, int? walletTypeId)
        {
           // return await _repo.GetWalletSummaryAsync(vendorId, from, to, walletTypeId);

            
                // 1️⃣ Get wallet summary
                var report = await _repo.GetWalletSummaryAsync(vendorId, from, to, walletTypeId);

                // 2️⃣ Get vendor details
                var vendor = await _repo.GetVendorContextAsync(vendorId);

                // 3️⃣ Populate header info
                report.VendorName = vendor.BusinessName;
                report.GSTIN = vendor.GSTNumber;
                report.Address = vendor.AddressPreview;

                return report;
            
        }
        public async Task<List<VendorWalletTypeDto>> ExecuteWalletTypeAsync(int vendorId)
            => await _repo.GetWalletTypesAsync(vendorId);
        
        


    }

}
