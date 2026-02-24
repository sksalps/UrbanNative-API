// Repositories/Vendors/SettlementPayoutRepository.cs
using Dapper;
using System.Data;
using UrbanNative.Application.DTOs.Vendors.Wallet;
using UrbanNative.Application.Interfaces.Vendors.Wallet;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories.Vendors
{
    public class SettlementPayoutRepository : ISettlementPayoutRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public SettlementPayoutRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<SettlementPayoutSummaryDto> GetSummaryAsync(string entityType, int entityId, string filterType, DateTime? fromDate, DateTime? toDate)
        {
            using var conn = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@EntityType", entityType);
            parameters.Add("@EntityID", entityId);
            parameters.Add("@FilterType", filterType);
            parameters.Add("@FromDate", fromDate);
            parameters.Add("@ToDate", toDate);

            return await conn.QueryFirstAsync<SettlementPayoutSummaryDto>(
                "sp_SettlementPayout_Summary",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<SettlementPayoutDto>> GetSettlementsAsync(string entityType, int entityId, string filterType, DateTime? fromDate, DateTime? toDate)
        {
            using var conn = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@EntityType", entityType);
            parameters.Add("@EntityID", entityId);
            parameters.Add("@FilterType", filterType);
            parameters.Add("@FromDate", fromDate);
            parameters.Add("@ToDate", toDate);

            return await conn.QueryAsync<SettlementPayoutDto>(
                "sp_SettlementPayout_GetGrid1",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<SettlementPaymentDto>> GetPaymentsAsync(long settlementId, string entityType, int entityId)
        {
            using var conn = _connectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@SettlementID", settlementId);
            parameters.Add("@EntityType", entityType);
            parameters.Add("@EntityID", entityId);

            return await conn.QueryAsync<SettlementPaymentDto>(
                "sp_SettlementPayments_GetGrid2",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}