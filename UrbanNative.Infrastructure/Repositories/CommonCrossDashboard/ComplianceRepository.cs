using Azure.Core;
using Dapper;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Data;
//using UrbanNative.Application.DTOs.Compliance;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Application.Interfaces.CommonCrossDashboard.Compliance;
using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories.CommonCrossDashboard
{
    public class ComplianceRepository : IComplianceRepository
    {
        
        private readonly SqlConnectionFactory _connFactory;
        private readonly IFileStorageService _fileUpload;
        

        public ComplianceRepository(
            SqlConnectionFactory connectionFactory,
            IFileStorageService fileUpload     )
        {
            _connFactory = connectionFactory;
            _fileUpload = fileUpload;
            
        }

        // 🔝 Dashboard Summary
        public async Task<IEnumerable<ComplianceScopeDashboardSummaryDto>> GetDashboardSummaryAsync(
            string entityType,
            int entityId)
        {
            using var conn = _connFactory.CreateConnection();

            return await conn.QueryAsync<ComplianceScopeDashboardSummaryDto>(
                "sp_Compliance_DashboardSummary",
                new { EntityType = entityType, EntityID = entityId },
                commandType: CommandType.StoredProcedure);
        }

        // 📊 Category Progress Strip
        public async Task<IEnumerable<ComplianceCategoryProgressDto>> GetCategoryProgressAsync(
            string entityType,
            int entityId)
        {
            using var conn = _connFactory.CreateConnection();

            return await conn.QueryAsync<ComplianceCategoryProgressDto>(
                "sp_Compliance_GroupProgress",
                new { EntityType = entityType, EntityID = entityId },
                commandType: CommandType.StoredProcedure);
        }

        // 📋 Category Grid-0
        public async Task<IEnumerable<ComplianceCategoryGridDto>> GetCategoryGridAsync(
            string entityType,
            int entityId)
        {
            using var conn = _connFactory.CreateConnection();

            return await conn.QueryAsync<ComplianceCategoryGridDto>(
                "sp_Compliance_GroupGrid",
                new { EntityType = entityType, EntityID = entityId },
                commandType: CommandType.StoredProcedure);
        }

        // 📄 Documents by Category (Grid-1)
        public async Task<IEnumerable<ComplianceDocumentDto>> GetDocumentsByCategoryAsync(
            string entityType,
            int entityId,
            int groupId)
        {
            using var conn = _connFactory.CreateConnection();

            return await conn.QueryAsync<ComplianceDocumentDto>(
                "sp_Compliance_DocumentsByGroup",
                new
                {
                    EntityType = entityType,
                    EntityID = entityId,
                    GroupID = groupId
                },
                commandType: CommandType.StoredProcedure);
        }

        // 📚 Document History (Grid-2)
        public async Task<IEnumerable<ComplianceDocumentHistoryDto>> GetDocumentHistoryAsync(
            string entityType,
            int entityId,
            int complianceId)
        {
            using var conn = _connFactory.CreateConnection();

            return await conn.QueryAsync<ComplianceDocumentHistoryDto>(
                "sp_Compliance_DocumentHistory",
                new
                {
                    EntityType = entityType,
                    EntityID = entityId,
                    ComplianceID = complianceId
                },
                commandType: CommandType.StoredProcedure);
        }



        /*public async Task UploadDocumentAsync(
        string entityType,
        int entityId,
        ComplianceUploadRequest request,
        IFormFile? file
        )
        {
            using var conn = _connFactory.CreateConnection();
            
            var fileUrl= await _fileUpload.UploadAsync(file, "compliance","", entityType, entityId);
            var fileName = file.FileName;
            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";

            if (request.UploadID > 0)
            {

                var ownershipStatus = await conn.QueryFirstOrDefaultAsync<string>(
                "SELECT Top 1 UploadID FROM ComplianceDocumentsUploaded WHERE EntityType=@EntityType AND EntityID=@EntityID AND ComplianceID=@ComplianceID AND IsActive=1 AND UploadID=@UploadID  order by UploadID desc",
                new { entityType, entityId, request.ComplianceID, request.UploadID });

                if (Convert.ToInt32(ownershipStatus) > 0)
                {
                    var existingStatus = await conn.QueryFirstOrDefaultAsync<string>(
                    "SELECT Top 1 verificationStatus FROM ComplianceDocumentsUploaded WHERE  UploadID=@UploadID  order by UploadID desc",
                    new {  request.UploadID });
                    if (existingStatus == "APPROVED")
                    {
                        throw new InvalidOperationException("Cannot update a Approved document. Please contact admin.");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Invalid Compliance selected");
                }
            }
            // 🔹 STEP 7: Fetch expiry policy from master
            var master = await conn.QueryFirstAsync<(bool HasExpiry, int? DefaultExpiryMonths)>(
                @"SELECT HasExpiry, DefaultExpiryMonths FROM ComplianceMaster WHERE ComplianceID = @ComplianceID",
                new { request.ComplianceID });

            // 🔹 Determine final expiry date
            DateTime? finalExpiry = request.ExpiryDate;

            if (master.HasExpiry && master.DefaultExpiryMonths.HasValue)
            {
                var systemExpiry = DateTime.Today.AddMonths(master.DefaultExpiryMonths.Value);

                // Rule: use earlier date if provided, otherwise system expiry
                if (!request.ExpiryDate.HasValue || request.ExpiryDate > systemExpiry)
                    finalExpiry = systemExpiry;
            }

            
            await conn.ExecuteAsync(
            "sp_ComplianceFormDocument_Upsert",
            new
            {
                @UploadID = @UploadID OUTPUT,
                EntityType = entityType,
                EntityID = entityId,
                ComplianceID = request.ComplianceID,
                FileName = uniqueFileName,
                FileURL = fileUrl,
                ExpiryDate = finalExpiry,
                DocumentNumber = request.DocumentNumber   // NEW

                
        },
            commandType: CommandType.StoredProcedure);
        }
        */

    public async Task<int> UploadDocumentAsync(string entityType,int entityId,ComplianceUploadRequest request,
    IFormFile? file)
    {
        using var conn = _connFactory.CreateConnection();

        // 🔥 STEP 1: VALIDATION (single query)
        if (request.UploadID > 0)
        {
            var existing = await conn.QueryFirstOrDefaultAsync<(int UploadID, string VerificationStatus)>(
                @"SELECT UploadID, VerificationStatus 
            FROM ComplianceDocumentsUploaded 
            WHERE UploadID = @UploadID 
            AND EntityType = @EntityType 
            AND EntityID = @EntityID 
            AND ComplianceID = @ComplianceID
            AND IsActive = 1",
                new
                {
                    request.UploadID,
                    entityType,
                    entityId,
                    request.ComplianceID
                });

            if (existing.UploadID == 0)
                throw new InvalidOperationException("Invalid Compliance selected");

            if (existing.VerificationStatus == "APPROVED")
                throw new InvalidOperationException("Cannot update an approved document. Please upload new.");
        }

        // 🔥 STEP 2: FETCH MASTER RULES
        var master = await conn.QueryFirstAsync<(bool HasExpiry, int? DefaultExpiryMonths)>(
            @"SELECT HasExpiry, DefaultExpiryMonths 
        FROM ComplianceMaster 
        WHERE ComplianceID = @ComplianceID",
            new { request.ComplianceID });

        // 🔥 STEP 3: EXPIRY LOGIC
        DateTime? finalExpiry = request.ExpiryDate;

        if (master.HasExpiry && master.DefaultExpiryMonths.HasValue)
        {
            var systemExpiry = DateTime.Today.AddMonths(master.DefaultExpiryMonths.Value);

            if (!request.ExpiryDate.HasValue || request.ExpiryDate > systemExpiry)
                finalExpiry = systemExpiry;
        }

        // 🔥 STEP 4: FILE UPLOAD (AFTER VALIDATION)
        string fileUrl = null;
        string fileName = null;

        if (file != null)
        {
            fileUrl = await _fileUpload.UploadAsync(file, "compliance", "", entityType, entityId);
            fileName = Path.GetFileName(fileUrl);
        }

        // 🔥 STEP 5: CALL SP WITH OUTPUT
        var parameters = new DynamicParameters();

        parameters.Add("@UploadID", request.UploadID, DbType.Int32, ParameterDirection.InputOutput);
        parameters.Add("@EntityType", entityType);
        parameters.Add("@EntityID", entityId);
        parameters.Add("@ComplianceID", request.ComplianceID);
        parameters.Add("@FileName", fileName);
        parameters.Add("@FileURL", fileUrl);
        parameters.Add("@ExpiryDate", finalExpiry);
        parameters.Add("@DocumentNumber", request.DocumentNumber);

        await conn.ExecuteAsync(
            "sp_ComplianceFormDocument_Upsert",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        // 🔥 STEP 6: RETURN OUTPUT ID
        return parameters.Get<int>("@UploadID");
    }
        public async Task<ComplianceMaster?> GetComplianceAsync(int complianceId)
        {
            using var conn = _connFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<ComplianceMaster>(
                "sp_ComplianceMaster_GetById",
                new { ComplianceID = complianceId },
                commandType: System.Data.CommandType.StoredProcedure
            );
        }

        // 🟢 Category Status Banner
        public async Task<ComplianceCategoryStatusDto> GetCategoryStatusAsync(string entityType,int entityId,int groupId)
        {
            using var conn = _connFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<ComplianceCategoryStatusDto>(
                "sp_Compliance_GroupStatusBanner",
                new
                {
                    EntityType = entityType,
                    EntityID = entityId,
                    GroupID = groupId
                },
                commandType: CommandType.StoredProcedure);
        }
        public async Task<int?> GetComplianceByNameAsync(string complianceName)
        {
            using var conn = _connFactory.CreateConnection();
            var ComplianceID = await conn.QueryFirstOrDefaultAsync<int>(
           "SELECT Top 1 ComplianceId FROM ComplianceMaster where ComplianceName=@complianceName AND IsActive=1",
           new { complianceName });

            if (ComplianceID ==0)
            {
                throw new InvalidOperationException("Bank Compliance Not Configured");
            }
            return ComplianceID;
        }

        //============Use this for Menu, Compliance=>Documents List==================//

        public async Task<IEnumerable<ComplianceDocumentListDto>> GetUploadedDocumentsAsync(string entityType, int entityId)
        {
            using var conn = _connFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@EntityType", entityType);
            parameters.Add("@EntityID", entityId);

            var result = await conn.QueryAsync<ComplianceDocumentListDto>(
                "sp_ComplianceDocumentsUploaded_List",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public async Task DeleteDocumentAsync(int uploadId, string entityType, int entityId)
        {
            using var connection = _connFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@UploadID", uploadId);
            parameters.Add("@EntityType", entityType);
            parameters.Add("@EntityID", entityId);

            await connection.ExecuteAsync(
                "sp_ComplianceDocumentsUploaded_Delete",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}

