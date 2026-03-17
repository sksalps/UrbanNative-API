using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using Microsoft.AspNetCore.Http;

namespace UrbanNative.Vendors.Services
{
    public interface IVendorComplianceService
    {
        Task<ComplianceDashboardSummaryDto?> GetDashboardSummaryAsync();

        Task<IEnumerable<ComplianceCategoryProgressDto>> GetCategoryProgressAsync();

        Task<IEnumerable<ComplianceCategoryGridDto>> GetCategoryGridAsync();

        Task<IEnumerable<ComplianceDocumentDto>> GetDocumentsByCategoryAsync(int groupId);

        Task<IEnumerable<ComplianceDocumentHistoryDto>> GetDocumentHistoryAsync(int complianceId);

        Task<ComplianceCategoryStatusDto?> GetCategoryStatusAsync(int groupId);
        Task UploadDocumentAsync(int complianceId, IFormFile file,DateTime? expiryDate,string? documentNumber);
        Task<string?> ExtractDocumentAsync(IFormFile file, string regex, int complianceId);
    }

}

