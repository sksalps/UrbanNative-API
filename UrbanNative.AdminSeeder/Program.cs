using System;
using System.Data;
using Microsoft.Data.SqlClient;                 // ✔ correct SQL client
using UrbanNative.Infrastructure.Security;      // ✔ PasswordHelper from Infrastructure

class Program
{
    static async Task Main()
    {
        Console.WriteLine("=== UrbanNative Admin Seeder ===");

        // TODO: Update these values before running:
        //string connectionString = "Server=server;Database=UrbanNativeDB;Trusted_Connection=True;TrustServerCertificate=True;";
        string connectionString = "Server=SERVER;Database=UrbanNativeDB;User Id=sa;Password=Alps@123456;TrustServerCertificate=True;";
        string username = "admin";
        string email = "admin@local.dev";
        string displayName = "Super Admin";
        string role = "Admin";
        string password = "Admin@123";   // CHANGE THIS BEFORE PRODUCTION

        // Generate secure hash + salt
        (byte[] hash, byte[] salt) = PasswordHelper.CreateHash(password);

        const string sql = @"
INSERT INTO dbo.AdminUsers (Username, Email, DisplayName, Role, PasswordHash, PasswordSalt, IsActive, CreatedAt)
VALUES (@username, @email, @displayName, @role, @passwordHash, @passwordSalt, 1, SYSUTCDATETIME());
SELECT SCOPE_IDENTITY();
";

        try
        {
            await using var con = new SqlConnection(connectionString);
            await using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.NVarChar, 256) { Value = username });
            cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar, 256) { Value = email });
            cmd.Parameters.Add(new SqlParameter("@displayName", SqlDbType.NVarChar, 256) { Value = displayName });
            cmd.Parameters.Add(new SqlParameter("@role", SqlDbType.NVarChar, 100) { Value = role });
            cmd.Parameters.Add(new SqlParameter("@passwordHash", SqlDbType.VarBinary, -1) { Value = hash });
            cmd.Parameters.Add(new SqlParameter("@passwordSalt", SqlDbType.VarBinary, -1) { Value = salt });

            await con.OpenAsync();
            var insertedId = await cmd.ExecuteScalarAsync();

            Console.WriteLine($"Admin user created successfully with Id = {insertedId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error while inserting admin:");
            Console.WriteLine(ex.ToString());     // ✔ shows full error, not only message
        }

        Console.WriteLine("=== Seeder Completed ===");
    }
}
