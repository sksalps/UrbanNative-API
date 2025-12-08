using System.Data;
using Microsoft.Data.SqlClient;
using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public UserRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> RegisterAsync(User user)
        {
            using SqlConnection conn = (SqlConnection)_connectionFactory.CreateConnection();
            using SqlCommand cmd = new SqlCommand("sp_RegisterUser", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Mobile", user.Mobile);
            cmd.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ReferredByCode", user.ReferredByCode ?? (object)DBNull.Value);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();

            return Convert.ToInt32(result);
        }

        public async Task<User?> LoginAsync(string mobile)
        {
            using SqlConnection conn = (SqlConnection)_connectionFactory.CreateConnection();
            using SqlCommand cmd = new SqlCommand("sp_LoginUser", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mobile", mobile);

            await conn.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (!reader.Read())
                return null;

            return new User
            {
                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Mobile = reader.GetString(reader.GetOrdinal("Mobile")),
                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                ReferralCode = reader.IsDBNull(reader.GetOrdinal("ReferralCode")) ? null : reader.GetString(reader.GetOrdinal("ReferralCode")),
                WalletBalance = reader.IsDBNull(reader.GetOrdinal("WalletBalance")) ? 0 : reader.GetDecimal(reader.GetOrdinal("WalletBalance"))
            };
        }
    }
}