using Dapper;
using System.Data;
using System.Runtime.InteropServices;
using UrbanNative.Application.DTOs.AdminInventory;
using UrbanNative.Application.GlobalCall.VariantValueSignature;
using UrbanNative.Application.Interfaces;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    public class AdminInventoryLogRepository : IAdminInventoryLogRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public AdminInventoryLogRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        public async Task<AdminInventoryLogPeriodResultDto>   GetLogsBySkuPeriodAsync(int skuId, DateTime fromDate, DateTime toDate)
        {
            using var conn = _connectionFactory.CreateConnection();

            // 🔹 SAME AS SKU ENGINE
            var variantNames = await GetVariantNamesAsync(conn);
            var valueNames = await GetVariantValueNamesAsync(conn);

            using var multi = await conn.QueryMultipleAsync(
                "sp_AdminInventoryLogs_BySKU_Period",
                new
                {
                    SKUId = skuId,
                    FromDate = fromDate,
                    ToDate = toDate
                },
                commandType: CommandType.StoredProcedure
            );

            var header = await multi.ReadSingleAsync<AdminInventoryLogHeaderDto>();
            var logs = (await multi.ReadAsync<AdminInventoryLogDto>()).ToList();
            var closing = await multi.ReadSingleAsync<int>();

            // 🔹 DECODE SIGNATURE HERE (KEY STEP)
            header.VariantDisplay =
                ValueSignatureDecoder.Decode(
                    header.ValueSignature,
                    variantNames,
                    valueNames
                );

            header.ClosingStock = closing;

            return new AdminInventoryLogPeriodResultDto
            {
                Header = header,
                Logs = logs
            };



        }



        private async Task<Dictionary<int, string>> GetVariantNamesAsync(IDbConnection conn)
        {
            var sql = "SELECT VariantID, VariantName FROM VariantMaster WHERE IsActive = 1";
            var rows = await conn.QueryAsync<(int VariantID, string VariantName)>(sql);
            return rows.ToDictionary(x => x.VariantID, x => x.VariantName);
        }

        private async Task<Dictionary<int, string>> GetVariantValueNamesAsync(IDbConnection conn)
        {
            var sql = "SELECT VariantValueID, ValueName FROM VariantValues WHERE IsActive = 1";
            var rows = await conn.QueryAsync<(int VariantValueID, string ValueName)>(sql);
            return rows.ToDictionary(x => x.VariantValueID, x => x.ValueName);
        }

    }
}


