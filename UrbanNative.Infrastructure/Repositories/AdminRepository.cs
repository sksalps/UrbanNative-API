using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace UrbanNative.Infrastructure.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly string _conn;

        public AdminRepository(IConfiguration config)
        {
            var cs = config.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(cs))
                throw new ArgumentNullException(nameof(config),
                    "Connection string 'Default' not found in configuration.");

            _conn = cs;
        }

        public async Task<AdminUser?> GetByUsernameOrEmailAsync(string identifier)
        {
            const string sql = @"
                SELECT TOP(1) Id, Username, Email, DisplayName, Role, PasswordHash, PasswordSalt, IsActive
                FROM AdminUsers
                WHERE (Username = @ident OR Email = @ident) AND IsActive = 1;
            ";

            await using var con = new SqlConnection(_conn);
            await using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.Add(new SqlParameter("@ident", SqlDbType.NVarChar, 256)
            {
                Value = identifier
            });

            await con.OpenAsync();

            await using var rdr = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow);
            if (!await rdr.ReadAsync())
                return null;

            var user = new AdminUser
            {
                Id = rdr.GetInt32(rdr.GetOrdinal("Id")),
                Username = rdr.GetString(rdr.GetOrdinal("Username")),
                Email = rdr.GetString(rdr.GetOrdinal("Email")),
                DisplayName = rdr.IsDBNull(rdr.GetOrdinal("DisplayName"))
                    ? null
                    : rdr.GetString(rdr.GetOrdinal("DisplayName")),
                Role = rdr.IsDBNull(rdr.GetOrdinal("Role"))
                    ? "Admin"
                    : rdr.GetString(rdr.GetOrdinal("Role")),
                IsActive = rdr.GetBoolean(rdr.GetOrdinal("IsActive"))
            };

            // password hash + salt
            int hashIdx = rdr.GetOrdinal("PasswordHash");
            int saltIdx = rdr.GetOrdinal("PasswordSalt");

            user.PasswordHash = rdr.IsDBNull(hashIdx)
                ? Array.Empty<byte>()
                : (byte[])rdr[hashIdx];

            user.PasswordSalt = rdr.IsDBNull(saltIdx)
                ? Array.Empty<byte>()
                : (byte[])rdr[saltIdx];

            return user;
        }
    }
}
