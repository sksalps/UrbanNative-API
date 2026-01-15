using Dapper;
using System.Data;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    public class VendorAuthRepository : IVendorAuthRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public VendorAuthRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<VendorLoginResultDto?> GetVendorForLoginAsync(string identifier)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<VendorLoginResultDto>(
                "sp_Vendor_Login",
                new { Identifier = identifier },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
