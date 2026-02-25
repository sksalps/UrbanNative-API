using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;


namespace UrbanNative.Vendors.Services
{
    public class VendorComplianceService: IVendorComplianceService
    {
        private readonly HttpClient _http;

        public VendorComplianceService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        // 🔝 Dashboard Summary
        public async Task<ComplianceDashboardSummaryDto?> GetDashboardSummaryAsync()
        {
            return await _http.GetFromJsonAsync<ComplianceDashboardSummaryDto>(
                "api/compliance/dashboard");
        }

        // 📊 Category Progress Strip
        public async Task<IEnumerable<ComplianceCategoryProgressDto>> GetCategoryProgressAsync()
        {
            return await _http.GetFromJsonAsync<IEnumerable<ComplianceCategoryProgressDto>>(
                "api/compliance/categories") ?? Enumerable.Empty<ComplianceCategoryProgressDto>();
        }

        // 📋 Category Grid
        public async Task<IEnumerable<ComplianceCategoryGridDto>> GetCategoryGridAsync()
        {
            return await _http.GetFromJsonAsync<IEnumerable<ComplianceCategoryGridDto>>(
                "api/compliance/categories") ?? Enumerable.Empty<ComplianceCategoryGridDto>();
        }

        // 📄 Documents by Category (Grid-1)
        public async Task<IEnumerable<ComplianceDocumentDto>> GetDocumentsByCategoryAsync(int groupId)
        {
            return await _http.GetFromJsonAsync<IEnumerable<ComplianceDocumentDto>>(
                $"api/compliance/categories/{groupId}/documents") ?? Enumerable.Empty<ComplianceDocumentDto>();
        }

        // 📚 Document History (Grid-2)
        public async Task<IEnumerable<ComplianceDocumentHistoryDto>> GetDocumentHistoryAsync(int complianceId)
        {
            return await _http.GetFromJsonAsync<IEnumerable<ComplianceDocumentHistoryDto>>(
                $"api/compliance/documents/{complianceId}/history") ?? Enumerable.Empty<ComplianceDocumentHistoryDto>();
        }

        // 🟢 Category Status Banner
        public async Task<ComplianceCategoryStatusDto?> GetCategoryStatusAsync(int groupId)
        {
            return await _http.GetFromJsonAsync<ComplianceCategoryStatusDto>(
                $"api/compliance/categories/{groupId}/status");
        }
    }

}

