using Microsoft.Data.SqlClient;
using System.Data;
using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    public class VariantMasterRepository : IVariantMasterRepository
    {
        private readonly SqlConnectionFactory _factory;

        public VariantMasterRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<int> AddVariantMasterAsync(string variantName)
        {
            using var conn = (SqlConnection)_factory.CreateConnection();
            using var cmd = new SqlCommand("sp_AddVariantMaster", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@VariantName", variantName);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<IEnumerable<VariantMaster>> GetAllVariantTypesAsync()
        {
            var list = new List<VariantMaster>();

            using var conn = (SqlConnection)_factory.CreateConnection();
            using var cmd = new SqlCommand(
                "SELECT VariantID, VariantName, IsActive FROM VariantMaster", conn);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new VariantMaster
                {
                    VariantID = reader.GetInt32(0),
                    VariantName = reader.GetString(1),
                    IsActive = reader.GetBoolean(2)
                });
            }

            return list;
        }
    }
}
