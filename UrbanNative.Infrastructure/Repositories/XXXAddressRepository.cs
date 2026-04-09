using System.Data;
using Dapper;
using UrbanNative.Application.DTOs.Address;
using UrbanNative.Application.Interfaces;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories
{
    public class XXXAddressRepository : IAddressRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public XXXAddressRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> SaveAddressAsync(AddressCreateDto dto)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.ExecuteScalarAsync<int>(
                "sp_Address_Save",
                new
                {
                    dto.EntityType,
                    dto.EntityID,
                    dto.AddressLine1,
                    dto.AddressLine2,
                    dto.Landmark,
                    dto.CountryID,
                    dto.StateID,
                    dto.CityID,
                    dto.Pincode,
                    dto.Latitude,
                    dto.Longitude,
                    dto.AddressType,
                    dto.IsPrimary
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<AddressDto>> GetAddressesAsync(string entityType, int entityId)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryAsync<AddressDto>(
                "sp_Address_GetByEntity",
                new
                {
                    EntityType = entityType,
                    EntityID = entityId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task DeactivateAddressAsync(int addressId)
        {
            using var conn = _connectionFactory.CreateConnection();

            await conn.ExecuteAsync(
                "sp_Address_Deactivate",
                new { AddressID = addressId },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}