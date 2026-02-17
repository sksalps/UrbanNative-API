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

        public async Task<IEnumerable<AccountHeadDto>> GetAccountHeadsAsync()
        {
            using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryAsync<AccountHeadDto>(
                "sp_WalletAccountHead_GetActive",
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
                    filter.SourceType,
                    filter.AccountHeadId,
                    filter.OrderNo
                },
                commandType: CommandType.StoredProcedure);
        return result;
        }
    }

}
