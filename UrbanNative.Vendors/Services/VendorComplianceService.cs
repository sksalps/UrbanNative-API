using AutoMapper;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Shared.Models.Compliance;
using System.Net.Http.Headers;
using UrbanNative.Vendors.Mapping.Profiles;



namespace UrbanNative.Vendors.Services
{
    public class VendorComplianceService: IVendorComplianceService
    {
        private readonly HttpClient _http;
        private readonly IMapper _mapper;

        public VendorComplianceService(IHttpClientFactory factory, IMapper mapper)
        {
            _http = factory.CreateClient("ApiClient");
            _mapper = mapper;
        }

        // 🔝 Dashboard Summary
        public async Task<IEnumerable<ComplianceScopeDashboardSummaryDto?>> GetDashboardSummaryAsync()
        {
            return await _http.GetFromJsonAsync < IEnumerable < ComplianceScopeDashboardSummaryDto >>("api/compliance/dashboard");
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
                "api/compliance/group") ?? Enumerable.Empty<ComplianceCategoryGridDto>();
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


        public async Task<string?> ExtractDocumentAsync( IFormFile file, string regex,int complianceId)
        {
            using var form = new MultipartFormDataContent();

            // File content
            var fileContent = new StreamContent(file.OpenReadStream());
            fileContent.Headers.ContentType =
                new MediaTypeHeaderValue(file.ContentType);

            form.Add(fileContent, "File", file.FileName);
            // Other fields
            form.Add(new StringContent(complianceId.ToString()), "ComplianceId");
            form.Add(new StringContent(regex ?? ""), "Regex");
            // API call
            var response = await _http.PostAsync("api/compliance/validate-compliance",form);

            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<ComplianceValidationResultDto>();

            if (result == null)
                return null;

            if (!result.IsValid)
                return result.Message;

            return result.DetectedNumber;
        }
        
        //=============================Menu Component - Uploaded Documents List=============================

        public async Task<List<ComplianceDocumentViewModel>> GetUploadedDocumentsListAsync()
        {
            var response = await _http.GetAsync("api/compliance/documents_list");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            var data = await response.Content.ReadFromJsonAsync<List<ComplianceDocumentListDto>>();

            return _mapper.Map<List<ComplianceDocumentViewModel>>(data ?? new());
        }

        public async Task DeleteDocumentAsync(int uploadId)
        {
            var response = await _http.DeleteAsync($"api/compliance/deletedoc?uploadId={uploadId}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }
    }

}

