using Dapper;
using System.Data;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces.Vendors;
using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Database;
using static QuestPDF.Helpers.Colors;

namespace UrbanNative.Infrastructure.Repositories.Vendors
{
    public class VendorSettingsRepository    : IVendorSettingsRepository
    {
        private readonly SqlConnectionFactory _db;

        public VendorSettingsRepository(           SqlConnectionFactory db)
        {
            _db = db;
        }

        public async Task<List<VendorSystemSettingDto>> GetAsync(int vendorId)
        {
            using var conn = _db.CreateConnection();

            var data = await conn.QueryAsync<VendorSystemSettingDto>(
                "sp_Vendor_SystemSettings_Get",
                new
                {
                    VendorId = vendorId
                },
                    commandType: CommandType.StoredProcedure);

            return data.ToList();
        }

        public async Task UpdateAsync(int systemSettingId,decimal value, DateTime effectiveFrom,  int vendorId)
        {
            using var conn = _db.CreateConnection();

            await conn.ExecuteAsync(
                "sp_Vendor_SystemSettings_Update",
                new
                {
                    SystemSettingId = systemSettingId,
                    NewValue = value,
                    EffectiveFrom = effectiveFrom,
                    VendorId = vendorId
                },

                commandType: CommandType.StoredProcedure);
        }
        public async Task<List<VendorSystemSettingHistoryDto>> GetHistoryAsync(int vendorId, int systemSettingId)
        {
            using var conn = _db.CreateConnection();

            var data = await conn.QueryAsync<VendorSystemSettingHistoryDto>(
                "sp_Vendor_SystemSetting_History",
                new
                {
                    VendorId = vendorId,
                    SystemSettingId = systemSettingId
                },
                    commandType: CommandType.StoredProcedure);

            return data.ToList();
        }
    }
}
