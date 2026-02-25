using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;

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
    }

}

