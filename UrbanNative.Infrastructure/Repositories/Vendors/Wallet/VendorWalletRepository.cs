using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Vendors.Logistics;
using UrbanNative.Application.DTOs.Vendors.Wallet;
using UrbanNative.Application.Interfaces.Vendors.Wallet;
using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories.Vendors.Wallet
{
    public class VendorWalletRepository : IVendorWalletRepository
    {
        
        private readonly SqlConnectionFactory _connectionFactory;

        public VendorWalletRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<AccountHeadDto>> GetAccountHeadsAsync(int VendorId)
        {
            using var conn = _connectionFactory.CreateConnection();
            var param = new DynamicParameters();
            param.Add("@OwnerId", VendorId);
            param.Add("@OwnerType", "VENDOR");
            return await conn.QueryAsync<AccountHeadDto>(
                "sp_WalletAccountHead_Lookup",
                 param,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<WalletLedgerRowDto>> GetWalletLedgerAsync(WalletLedgerFilterDto filter)
        {
            using var conn = _connectionFactory.CreateConnection();
            var result = await conn.QueryAsync<WalletLedgerRowDto>(
                "sp_VendorWalletLedger_Get",
            
                new
                {
                    filter.VendorId,
                    filter.FromDate,
                    filter.ToDate,
                    filter.TxnType,
                    filter.FinancialEvent,
                    filter.AccountHeadId,
                    filter.OrderNo
                },
                commandType: CommandType.StoredProcedure);
        return result;
        }

        public async Task<WalletLedgerSummaryDto> GetLedgerSummaryAsync(int vendorId, DateTime fromDate, DateTime toDate)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<WalletLedgerSummaryDto>(
                "sp_VendorWalletLedger_Summary",
                new
                {
                    VendorId = vendorId,
                    FromDate = fromDate,
                    ToDate = toDate
                },
                commandType: CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<string>> SearchOrdersAsync(int vendorId, string term)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryAsync<string>(
                "sp_VendorWallet_Order_Search",
                new { VendorId = vendorId, Term = term },
                commandType: CommandType.StoredProcedure);
        }


    }

}
