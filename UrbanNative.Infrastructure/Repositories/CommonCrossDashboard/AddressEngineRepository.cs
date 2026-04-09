using Dapper;
using Microsoft.AspNetCore.Connections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Address;
using UrbanNative.Application.DTOs.CommonCrossDashboard;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces.CommonCrossDashboard;
using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories.CommonCrossDashboard
{

    public class AddressEngineRepository : IAddressEngineRepository
    {
        
        private readonly SqlConnectionFactory _db;

            public AddressEngineRepository(SqlConnectionFactory db)
            {
                _db = db;
            }

            public async Task<List<AddressListDto>> GetListAsync(string entityType, int entityId, string addressType)
            {
                using var conn = _db.CreateConnection();
                var param = new { EntityType = entityType, EntityID = entityId };

                return (await conn.QueryAsync<AddressListDto>(
                    "sp_AddressMaster_Lookup", param, commandType: CommandType.StoredProcedure)).ToList();
            
            }

            public async Task<AddressListDto> GetByIdAsync(int addressId, string entityType, int entityId)
            {
                using var conn = _db.CreateConnection();

                return await conn.QueryFirstOrDefaultAsync<AddressListDto>(
                    "sp_AddressMaster_GetById",
                    new { AddressID = addressId, EntityType=entityType, EntityID=entityId },
                    commandType: CommandType.StoredProcedure
                );
            }

            public async Task<int> SaveAsync(AddressSaveDto dto, string entityType, int entityId)
            {
                using var conn = _db.CreateConnection();

                return await conn.ExecuteScalarAsync<int>(
                    "sp_AddressMaster_Upsert",
                    new
                    {
                        dto.AddressID,
                        EntityType = entityType,
                        EntityID = entityId,
                        dto.AddressLine1,
                        dto.AddressLine2,
                        dto.Landmark,
                        dto.Country,
                        dto.State,
                        dto.City,
                        dto.Pincode,
                        dto.AddressType,
                        dto.IsPrimary,
                        dto.AddressNickName
                        
                    },
                    commandType: CommandType.StoredProcedure
                );
            }

        public async Task<IEnumerable<CountryDto>> GetCountriesAsync(int? countryId)
        {
            using var conn = _db.CreateConnection();

            return await conn.QueryAsync<CountryDto>(
                "sp_AddressCountry_Lookup",
                new { CountryID = countryId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<StateDto>> GetStatesAsync(int? countryId, int? stateId)
        {
            using var conn = _db.CreateConnection();

            return await conn.QueryAsync<StateDto>(
                "sp_AddressState_Lookup",
                new { CountryID = countryId, StateID = stateId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CityDto>> GetCitiesAsync(int? stateId, int? cityId)
        {
            using var conn = _db.CreateConnection();

            return await conn.QueryAsync<CityDto>(
                "sp_AddressCity_Lookup",
                new { StateID = stateId, CityID = cityId },
                commandType: CommandType.StoredProcedure);
        }
    }
}
