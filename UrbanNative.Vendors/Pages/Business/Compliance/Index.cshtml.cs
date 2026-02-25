using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Vendors.Services;

namespace UrbanNative.Vendors.Pages.Business.Compliance
{
    public class IndexModel : PageModel
    {
        private readonly IVendorComplianceService _complianceService;

        public ComplianceDashboardSummaryDto? Summary { get; set; }
        public IEnumerable<ComplianceCategoryProgressDto> Categories { get; set; } = [];
        public IEnumerable<ComplianceCategoryGridDto> CategoryGrid { get; set; } = [];
        public ComplianceCategoryStatusDto? CategoryStatus { get; set; }
        public IEnumerable<ComplianceDocumentDto> Documents { get; set; } = [];

        // ✅ NEW: Document History (Grid-2)
        public IEnumerable<ComplianceDocumentHistoryDto> History { get; set; } = [];

        [BindProperty(SupportsGet = true)]
        public int? SelectedGroupId { get; set; }

        // ✅ NEW: Selected Document
        [BindProperty(SupportsGet = true)]
        public int? SelectedComplianceId { get; set; }

        public IndexModel(IVendorComplianceService complianceService)
        {
            _complianceService = complianceService;
        }

        public async Task OnGetAsync()
        {
            Summary = await _complianceService.GetDashboardSummaryAsync();
            Categories = await _complianceService.GetCategoryProgressAsync();
            CategoryGrid = await _complianceService.GetCategoryGridAsync();

            if (SelectedGroupId.HasValue)
            {
                Documents = await _complianceService.GetDocumentsByCategoryAsync(SelectedGroupId.Value);
                CategoryStatus = await _complianceService.GetCategoryStatusAsync(SelectedGroupId.Value);
            }

            // ✅ NEW: Load history when a document is clicked
            if (SelectedComplianceId.HasValue)
            {
                History = await _complianceService.GetDocumentHistoryAsync(SelectedComplianceId.Value);
            }

        }
    }
}