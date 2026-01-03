using System.Data;
using Dapper;
using UrbanNative.Application.DTOs.AdminInventory;
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

        public async Task<AdminInventoryLogPeriodResultDto> GetLogsBySkuPeriodAsync(      int skuId, DateTime fromDate, DateTime toDate)
        {
            using var conn = _connectionFactory.CreateConnection();

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

            var openingStock = await multi.ReadFirstOrDefaultAsync<int?>() ?? 0;
            var logs = await multi.ReadAsync<AdminInventoryLogDto>();
            var closingStock = await multi.ReadFirstOrDefaultAsync<int?>() ?? openingStock;

            return new AdminInventoryLogPeriodResultDto
            {
                OpeningStock = openingStock,
                ClosingStock = closingStock,
                Logs = logs
            };
        }
    }
}
