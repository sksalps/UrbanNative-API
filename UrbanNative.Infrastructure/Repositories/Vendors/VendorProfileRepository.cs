using System.Data;
using Dapper;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories.Vendors
{
    public class VendorProfileRepository : IVendorProfileRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public VendorProfileRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<VendorProfileDto> GetProfileAsync(int vendorId)
        {
            using var conn = _connectionFactory.CreateConnection();

            var data = await conn.QueryFirstOrDefaultAsync<VendorProfileDto>(
                "sp_Vendor_Profile_Get",
                new { VendorId = vendorId },
                commandType: CommandType.StoredProcedure
            );
            return data;
        }

        public async Task UpdateProfileAsync(int vendorId, VendorProfileDto profile)
        {
            using var conn = _connectionFactory.CreateConnection();

            await conn.ExecuteAsync(
                "sp_Vendor_Profile_Update",
                new
                {
                    VendorId = vendorId,
                    profile.BusinessName,
                    profile.ContactPerson,
                    profile.Email
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
