using Microsoft.AspNetCore.Http;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;

namespace UrbanNative.Application.Interfaces.UseCases.CommonCrossDashboard.Compliance

{
    public interface IComplianceUseCase
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

        Task<ComplianceCategoryStatusDto> GetCategoryStatusAsync(
            string entityType,
            int entityId,
            int groupId);
        Task<int> UploadDocumentAsync(string entityType, int entityId, ComplianceUploadRequest request,
            IFormFile? file);
        Task<string?> ExtractDocumentNumberAsync(Stream fileStream, string regex);

      //============Use this for Menu, Compliance=>Documents List==================//
        Task<IEnumerable<ComplianceDocumentListDto>> GetUploadedDocumentsAsync(string entityType, int entityId);
        Task DeleteDocumentAsync(int uploadId, string entityType, int entityId);
    }
}