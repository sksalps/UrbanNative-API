using Microsoft.Data.SqlClient;
using System.Data;
using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    public class CategoryVariantRepository : ICategoryVariantRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public CategoryVariantRepository(SqlConnectionFactory factory)
        {
            _connectionFactory = factory;
        }

        public async Task<int> AssignVariantToCategoryAsync(int categoryId, int variantId, int createdBy)
        {
            using var con = (SqlConnection)_connectionFactory.CreateConnection();
            //using var con = (SqlConnection)_factory.CreateConnection();
            using var cmd = new SqlCommand("sp_AssignVariantToCategory", con);

            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@CategoryID", categoryId);
            cmd.Parameters.AddWithValue("@VariantID", variantId);
            cmd.Parameters.AddWithValue("@CreatedBy", createdBy);

            await con.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<bool> RemoveVariantFromCategoryAsync(int categoryId, int variantId)
        {
            using var con = (SqlConnection)_connectionFactory.CreateConnection();
            using var cmd = new SqlCommand("sp_RemoveVariantFromCategory", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CategoryID", categoryId);
            cmd.Parameters.AddWithValue("@VariantID", variantId);

            await con.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result) == 1;
        }

        public async Task<IEnumerable<VariantMaster>> GetVariantsByCategoryAsync(int categoryId)
        {
            var list = new List<VariantMaster>();

            using var con = (SqlConnection)_connectionFactory.CreateConnection();
            using var cmd = new SqlCommand("sp_GetVariantsByCategory", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CategoryID", categoryId);

            await con.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new VariantMaster
                {
                    VariantID = reader.GetInt32(0),
                    VariantName = reader.GetString(1)
                });
            }

            return list;
        }
    }
}