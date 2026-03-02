using Azure.Core;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using static System.Net.WebRequestMethods;


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

        public async Task UploadDocumentAsync(
        int complianceId,
        IFormFile file,
        DateTime? expiryDate,
        string? documentNumber)
        {
            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(complianceId.ToString()), "complianceId");

            if (expiryDate.HasValue)
                content.Add(new StringContent(expiryDate.Value.ToString("yyyy-MM-dd")), "expiryDate");

            if (!string.IsNullOrWhiteSpace(documentNumber))
                content.Add(new StringContent(documentNumber), "documentNumber");

            content.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);

            var response = await _http.PostAsync("api/compliance/documents/upload", content);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Upload failed: {error}");
            }
            response.EnsureSuccessStatusCode();
        }
    }

}

