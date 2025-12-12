using Microsoft.Data.SqlClient;
using System.Data;
using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly SqlConnectionFactory _factory;

        public InventoryRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        // 1️⃣ STOCK IN
        public async Task<int> StockInAsync(int variantSetId, int quantity, int createdBy)
        {
            using var conn = (SqlConnection)_factory.CreateConnection();
            using var cmd = new SqlCommand("sp_StockIn", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@VariantSetID", variantSetId);
            cmd.Parameters.AddWithValue("@Quantity", quantity);
            cmd.Parameters.AddWithValue("@CreatedBy", createdBy);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        // 2️⃣ STOCK OUT
        public async Task<int> StockOutAsync(int variantSetId, int quantity, int createdBy)
        {
            using var conn = (SqlConnection)_factory.CreateConnection();
            using var cmd = new SqlCommand("sp_StockOut", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@VariantSetID", variantSetId);
            cmd.Parameters.AddWithValue("@Quantity", quantity);
            cmd.Parameters.AddWithValue("@CreatedBy", createdBy);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);  // -1 = insufficient stock
        }

        // 3️⃣ GET INVENTORY LOGS
        public async Task<IEnumerable<InventoryLog>> GetInventoryLogsAsync(int variantSetId)
        {
            var logs = new List<InventoryLog>();

            using var conn = (SqlConnection)_factory.CreateConnection();
            using var cmd = new SqlCommand("sp_GetInventoryLogs", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@VariantSetID", variantSetId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                logs.Add(new InventoryLog
                {
                    LogID = reader.GetInt32(0),
                    VariantSetID = reader.GetInt32(1),
                    ChangeType = reader.GetString(2),
                    Quantity = reader.GetInt32(3),
                    OldStock = reader.GetInt32(4),
                    NewStock = reader.GetInt32(5),
                    CreatedAt = reader.GetDateTime(6),
                    CreatedBy = reader.IsDBNull(7) ? null : reader.GetInt32(7)
                });
            }

            return logs;
        }

        // 4️⃣ CHECK LOW STOCK
        public async Task<LowStockResult?> CheckLowStockAsync(int variantSetId)
        {
            using var conn = (SqlConnection)_factory.CreateConnection();
            using var cmd = new SqlCommand("sp_CheckLowStock", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@VariantSetID", variantSetId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new LowStockResult
                {
                    VariantSetID = reader.GetInt32(0),
                    Stock = reader.GetInt32(1),
                    ReorderLevel = reader.GetInt32(2),
                    IsLowStock = reader.GetInt32(3) == 1
                };
            }

            return null;
        }
    }
}