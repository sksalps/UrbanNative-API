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

        // ================= UPSERT COMPLIANCE =================
        public async Task<int> UpsertComplianceAsync(
            int uploadId,
            string entityType,
            int entityId,
            int complianceId,
            string? fileName,
            string? fileUrl,
            string? documentNumber)
        {
            using var conn = _connectionFactory.CreateConnection();

            var result = await conn.QuerySingleAsync<int>(
                "sp_ComplianceBankDetails_Upsert",
                new
                {
                    UploadID = uploadId,
                    EntityType = entityType,
                    EntityID = entityId,
                    ComplianceID = complianceId,
                    FileName = fileName,
                    FileURL = fileUrl,
                    DocumentNumber = documentNumber
                },
                commandType: CommandType.StoredProcedure
            );

            return result;
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
                    ComplianceUploadId = dto.UploadId
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}