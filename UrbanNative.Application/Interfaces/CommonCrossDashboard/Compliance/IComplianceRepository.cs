using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Domain.Entities;


namespace UrbanNative.Application.Interfaces.CommonCrossDashboard.Compliance

{
    public interface IComplianceRepository
    {
        Task<IEnumerable<ComplianceScopeDashboardSummaryDto>> GetDashboardSummaryAsync(
            string entityType,
            int entityId);

        Task<IEnumerable<ComplianceCategoryProgressDto>> GetCategoryProgressAsync(
            string entityType,
            int entityId);

        Task<IEnumerable<ComplianceCategoryGridDto>> GetCategoryGridAsync(
            string entityType,
            int entityId);

        Task<IEnumerable<ComplianceDocumentDto>> GetDocumentsByCategoryAsync(
            string entityType,
            int entityId,
            int groupId);

        Task<IEnumerable<ComplianceDocumentHistoryDto>> GetDocumentHistoryAsync(
            string entityType,
            int entityId,
            int complianceId);

        Task UploadDocumentAsync(string entityType,int entityId,ComplianceUploadRequest request,
        Stream fileStream,string fileName);

        Task<ComplianceCategoryStatusDto> GetCategoryStatusAsync(string entityType, int entityId, int groupId);
        Task<ComplianceMaster?> GetComplianceAsync(int complianceId);
        Task<int?> GetComplianceByNameAsync(string complianceName);

    }

}