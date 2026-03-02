using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Vendors.Services;

namespace UrbanNative.Vendors.Pages.Business.Compliance
{
    public class IndexModel : PageModel
    {
        

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
        [BindProperty]
        public ComplianceUploadRequest UploadRequest { get; set; } = new();

        [BindProperty]
        public IFormFile File { get; set; } = default!;

        [BindProperty]
        public int ComplianceID { get; set; }

        [BindProperty]
        public DateTime? ExpiryDate { get; set; }
        [BindProperty]
        public string? DocumentNumber { get; set; }
        public IEnumerable<string> MissingMandatoryDocs { get; set; } = [];
        public string ApiBaseUrl { get; private set; } = "";

        //Constructor
        private readonly IVendorComplianceService _complianceService;
        private readonly IConfiguration _config;
        public IndexModel(IVendorComplianceService complianceService, IConfiguration config)
        {
            _complianceService = complianceService;
            _config = config;
            ApiBaseUrl = _config["ApiSettings:BaseUrl"] ?? "";
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
                if (Model.CategoryStatus?.MissingMandatoryDocs > 0)
                {
                    MissingMandatoryDocs = Model.Documents
                        .Where(d => d.IsMandatoryInGroup && d.VerificationStatus != "APPROVED")
                        .Select(d => d.ComplianceName)
                        .ToList();
                }

            }

            // ✅ NEW: Load history when a document is clicked
            if (SelectedComplianceId.HasValue)
            {
                History = await _complianceService.GetDocumentHistoryAsync(SelectedComplianceId.Value);
            }

        }
        

        public async Task<IActionResult> OnPostUploadAsync()
        {
            await _complianceService.UploadDocumentAsync(
                ComplianceID,
                File,
                ExpiryDate,
                DocumentNumber);

            return RedirectToPage(new { SelectedGroupId });
        }
    }
}