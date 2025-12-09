using Microsoft.Data.SqlClient;
using System.Data;
using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    public class ProductVariantValuesRepository : IProductVariantValuesRepository
    {
        private readonly SqlConnectionFactory _factory;

        public ProductVariantValuesRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<bool> AddValueToVariantSetAsync(int variantSetId, int valueId)
        {
            using var con = (SqlConnection)_factory.CreateConnection();
            using var cmd = new SqlCommand("sp_AddVariantMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@VariantSetID", variantSetId);
            cmd.Parameters.AddWithValue("@ValueID", valueId);

            await con.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result) == 1;
        }

        public async Task<IEnumerable<ProductVariantValues>> GetValuesByVariantSetAsync(int variantSetId)
        {
            var list = new List<ProductVariantValues>();

            using var con = (SqlConnection)_factory.CreateConnection();
            
            using var cmd = new SqlCommand(
                "SELECT ID, ValueID FROM ProductVariantValues WHERE VariantSetID = @VariantSetID", con);

            cmd.Parameters.AddWithValue("@VariantSetID", variantSetId);

            await con.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new ProductVariantValues
                {
                    ID = reader.GetInt32(0),
                    VariantSetID = variantSetId,
                    ValueID = reader.GetInt32(1)
                });
            }

            return list;
        }
    }
}