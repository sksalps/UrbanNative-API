using Dapper;
using System.Data;
//using UrbanNative.Application.DTOs.Compliance;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Application.Interfaces.CommonCrossDashboard.Compliance;
using UrbanNative.Infrastructure.Database;

namespace UrbanNative.Infrastructure.Repositories.CommonCrossDashboard
{
    public class ComplianceRepository : IComplianceRepository
    {
        
        private readonly SqlConnectionFactory _connFactory;

        public ComplianceRepository(SqlConnectionFactory connectionFactory)
        {
            _connFactory = connectionFactory;
        }

        // 🔝 Dashboard Summary
        public async Task<ComplianceDashboardSummaryDto> GetDashboardSummaryAsync(
            string entityType,
            int entityId)
        {
            using var conn = _connFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<ComplianceDashboardSummaryDto>(
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
                "sp_Compliance_CategoryProgress",
                new { EntityType = entityType, EntityID = entityId },
                commandType: CommandType.StoredProcedure);
        }

        // 📋 Category Grid
        public async Task<IEnumerable<ComplianceCategoryGridDto>> GetCategoryGridAsync(
            string entityType,
            int entityId)
        {
            using var conn = _connFactory.CreateConnection();

            return await conn.QueryAsync<ComplianceCategoryGridDto>(
                "sp_Compliance_CategoryGrid",
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
                "sp_Compliance_DocumentsByCategory",
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

        // 🟢 Category Status Banner
        public async Task<ComplianceCategoryStatusDto> GetCategoryStatusAsync(
            string entityType,
            int entityId,
            int groupId)
        {
            using var conn = _connFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<ComplianceCategoryStatusDto>(
                "sp_Compliance_CategoryStatusBanner",
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

