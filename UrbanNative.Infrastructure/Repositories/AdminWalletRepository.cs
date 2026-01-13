using Dapper;
using System.Data;
using UrbanNative.Application.DTOs.AdminWallet;
using UrbanNative.Application.Interfaces;
using UrbanNative.Infrastructure.Database;


namespace UrbanNative.Infrastructure.Repositories
{
    public class AdminWalletRepository : IAdminWalletRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public AdminWalletRepository(SqlConnectionFactory factory)
        {
            _connectionFactory = factory;
        }

        public async Task<IEnumerable<WalletLedgerDto>> GetLedgerAsync(
        string ownerType,
        int ownerId,
        string? walletType,
        string? accHead,
        string? txnType,
        string? sourceType,
        int? sourceId,
        DateTime? from,
        DateTime? to)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryAsync<WalletLedgerDto>(
                "sp_AdminWallet_GetLedger",
                new
                {
                    OwnerType = ownerType,
                    OwnerId = ownerId,
                    WalletType = walletType,
                    AccHead = accHead,
                    TxnType = txnType,
                    SourceType = sourceType,
                    SourceId = sourceId,
                    FromDate = from,
                    ToDate = to
                },
                commandType: CommandType.StoredProcedure);
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
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QuerySingleAsync<WalletBalanceSummaryDto>(
                "sp_AdminWallet_GetBalanceSummary",
                new
                {
                    OwnerType = ownerType,
                    OwnerId = ownerId,
                    WalletType = walletType,
                    AccHead = accHead,
                    TxnType = txnType,
                    SourceType = sourceType,
                    SourceId = sourceId,
                    FromDate = from,
                    ToDate = to
                },
                commandType: CommandType.StoredProcedure
            );
        }

    }

}
