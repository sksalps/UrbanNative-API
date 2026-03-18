using Dapper;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
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
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public ComplianceRepository(
            SqlConnectionFactory connectionFactory,
            IWebHostEnvironment env,
            IConfiguration config)
        {
            _connFactory = connectionFactory;
            _env = env;
            _config = config;
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

        

        public async Task UploadDocumentAsync(
        string entityType,
        int entityId,
        ComplianceUploadRequest request,
        Stream fileStream,
        string fileName)
        {
            using var conn = _connFactory.CreateConnection();

            var basePath = _config["FileStorage:BasePath"];

            if (!Path.IsPathRooted(basePath))
            {
                basePath = Path.Combine(_env.ContentRootPath, basePath);
            }

            var folderPath = Path.Combine(
                basePath,
                "compliance",
                entityType,
                entityId.ToString());

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var fullPath = Path.Combine(folderPath, uniqueFileName);

            using (var fileStreamOut = new FileStream(fullPath, FileMode.Create))
            {
                await fileStream.CopyToAsync(fileStreamOut);
            }
            var existingStatus = await conn.QueryFirstOrDefaultAsync<string>(
            "SELECT Top 1 VerificationStatus FROM ComplianceDocumentsUploaded WHERE EntityType=@EntityType AND EntityID=@EntityID AND ComplianceID=@ComplianceID AND IsActive=1 order by UploadID desc",
            new { entityType, entityId, request.ComplianceID });

            if (existingStatus == "APPROVED")
            {
                throw new InvalidOperationException("Document already approved. Re-upload not allowed.");
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

            var fileUrl = $"{folderPath}/{uniqueFileName}";
            await conn.ExecuteAsync(
            "sp_ComplianceDocument_Upload",
            new
            {
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
        
    }
}

