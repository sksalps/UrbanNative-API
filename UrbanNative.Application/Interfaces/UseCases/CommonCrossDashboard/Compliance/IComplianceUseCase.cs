using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;

namespace UrbanNative.Application.Interfaces.UseCases.CommonCrossDashboard.Compliance

{
    public interface IComplianceUseCase
    {
        Task<ComplianceDashboardSummaryDto> GetDashboardSummaryAsync(
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

        Task<ComplianceCategoryStatusDto> GetCategoryStatusAsync(
            string entityType,
            int entityId,
            int groupId);
    }
}