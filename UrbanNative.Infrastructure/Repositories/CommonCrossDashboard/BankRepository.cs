using Dapper;
using System.Data;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces.CommonCrossDashboard.Compliance;
using UrbanNative.Application.Interfaces.Vendors;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories.CommonCrossDashboard.Compliance
{
    public class BankRepository : IBankRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public BankRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        // ================= SAVE BANK =================
        public async Task SaveBankAsync(BankSaveRequestDto dto)
        {
            using var conn = _connectionFactory.CreateConnection();

            await conn.ExecuteAsync(
                "sp_ComplianceBankDetails_Save",
                new
                {
                    dto.BankID,
                    dto.EntityType,
                    dto.EntityID,
                    dto.AccountHolderName,
                    dto.BankName,
                    dto.BranchName,
                    dto.AccountNo,
                    dto.AccountType,
                    dto.UPIId,
                    dto.IFSCCode,
                    dto.CountryName,
                    dto.StateName,
                    dto.CityName,
                    dto.Pincode,
                    dto.IsPrimary,
                    dto.ComplianceId,
                    dto.FileName,
                    dto.FileURL,
                    DocumentNumber=dto.AccountNo
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}