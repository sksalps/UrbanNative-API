using Microsoft.Data.SqlClient;
using System.Data;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public MessageRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<string> GetMessageAsync(string key, string language = "en")
        {
            using var conn = (SqlConnection)_connectionFactory.CreateConnection();
            using var cmd = new SqlCommand(@"
                SELECT TOP 1 MessageText 
                FROM SystemMessages 
                WHERE MessageKey = @key AND LanguageCode = @lang
            ", conn);

            cmd.Parameters.AddWithValue("@key", key);
            cmd.Parameters.AddWithValue("@lang", language);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();

            return result?.ToString() ?? $"[{key}]";
        }
    }
}

