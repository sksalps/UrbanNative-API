using Dapper;
using UrbanNative.Application.GlobalCall.VariantValueSignature;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Caching
{
    public class VariantMasterCacheLoader
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public VariantMasterCacheLoader(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<VariantMasterCache> LoadAsync()
        {
            using var conn = _connectionFactory.CreateConnection();

            var variants = await conn.QueryAsync<(int Id, string Name)>(
                "SELECT VariantID, VariantName FROM VariantMaster WHERE IsActive = 1");

            var values = await conn.QueryAsync<(int Id, string Name)>(
                "SELECT VariantValueID, ValueName FROM VariantValues WHERE IsActive = 1");

            return new VariantMasterCache(
                variants.ToDictionary(x => x.Id, x => x.Name),
                values.ToDictionary(x => x.Id, x => x.Name)
            );
        }
    }
}
