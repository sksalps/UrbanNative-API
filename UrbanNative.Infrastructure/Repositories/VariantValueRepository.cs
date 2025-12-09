using Microsoft.Data.SqlClient;
using System.Data;
using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    public class VariantValueRepository : IVariantValueRepository
    {
        private readonly SqlConnectionFactory _factory;

        public VariantValueRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<int> AddVariantValueAsync(int variantId, string valueName)
        {
            using var con = (SqlConnection)_factory.CreateConnection();
            using var cmd = new SqlCommand("sp_AddVariantMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@VariantID", variantId);
            cmd.Parameters.AddWithValue("@ValueName", valueName);

            await con.OpenAsync();
            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        public async Task<IEnumerable<VariantValue>> GetValuesByVariantAsync(int variantId)
        {
            var list = new List<VariantValue>();

            using var con = (SqlConnection)_factory.CreateConnection();
            
            using var cmd = new SqlCommand(
                "SELECT ValueID, ValueName, IsActive FROM VariantValues WHERE VariantID = @VariantID", con);

            cmd.Parameters.AddWithValue("@VariantID", variantId);

            await con.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new VariantValue
                {
                    ValueID = reader.GetInt32(0),
                    ValueName = reader.GetString(1),
                    IsActive = reader.GetBoolean(2),
                    VariantID = variantId
                });
            }

            return list;
        }
    }
}