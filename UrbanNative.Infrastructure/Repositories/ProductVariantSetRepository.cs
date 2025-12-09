using Microsoft.Data.SqlClient;
using System.Data;
using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    public class ProductVariantSetRepository : IProductVariantSetRepository
    {
        private readonly SqlConnectionFactory _factory;

        public ProductVariantSetRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<int> AddVariantSetAsync(ProductVariantSet set)
        {
            using var con = (SqlConnection)_factory.CreateConnection();
            using var cmd = new SqlCommand("sp_AddVariantMaster", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ProductID", set.ProductID);
            cmd.Parameters.AddWithValue("@SKU", set.SKU ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@VariantPrice", set.VariantPrice);
            cmd.Parameters.AddWithValue("@Stock", set.Stock);

            await con.OpenAsync();
            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        public async Task<IEnumerable<ProductVariantSet>> GetVariantSetsByProductAsync(int productId)
        {
            var list = new List<ProductVariantSet>();

            using var con = (SqlConnection)_factory.CreateConnection();
            
            using var cmd = new SqlCommand(
                "SELECT VariantSetID, SKU, VariantPrice, Stock, IsActive, CreatedAt FROM ProductVariantSet WHERE ProductID = @ProductID", con);

            cmd.Parameters.AddWithValue("@ProductID", productId);

            await con.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new ProductVariantSet
                {
                    VariantSetID = reader.GetInt32(0),
                    SKU = reader.IsDBNull(1) ? null : reader.GetString(1),
                    VariantPrice = reader.GetDecimal(2),
                    Stock = reader.GetInt32(3),
                    IsActive = reader.GetBoolean(4),
                    CreatedAt = reader.GetDateTime(5),
                    ProductID = productId
                });
            }

            return list;
        }
    }
}