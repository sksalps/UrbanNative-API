using Microsoft.AspNetCore.Http;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Shared.Models.Compliance;

namespace UrbanNative.Vendors.Services
{
    public interface IVendorComplianceService
    {
        Task<IEnumerable<ComplianceScopeDashboardSummaryDto?>> GetDashboardSummaryAsync();

        Task<IEnumerable<ComplianceCategoryProgressDto>> GetCategoryProgressAsync();

        Task<IEnumerable<ComplianceCategoryGridDto>> GetCategoryGridAsync();

        Task<IEnumerable<ComplianceDocumentDto>> GetDocumentsByCategoryAsync(int groupId);

        Task<IEnumerable<ComplianceDocumentHistoryDto>> GetDocumentHistoryAsync(int complianceId);

        Task<ComplianceCategoryStatusDto?> GetCategoryStatusAsync(int groupId);
        Task UploadDocumentAsync(int complianceId, IFormFile file,DateTime? expiryDate,string? documentNumber);
        Task<string?> ExtractDocumentAsync(IFormFile file, string regex, int complianceId);

        Task<List<ComplianceDocumentViewModel>> GetUploadedDocumentsListAsync();
        Task DeleteDocumentAsync(int uploadId);
    }

}

