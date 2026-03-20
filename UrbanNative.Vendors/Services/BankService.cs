using System.Net.Http.Json;
using System.Net.Http.Headers;
using UrbanNative.Vendors.Services.Interfaces;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;

 namespace UrbanNative.Vendors.Services
{
    public class BankService : IBankService
    {
        private readonly HttpClient _http;

        public BankService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        // ================= OCR + VALIDATE + UPLOAD =================

        public async Task<ComplianceValidationResultDto> ExtractOnlyAsync(IFormFile file)
        {
            using var content = new MultipartFormDataContent();

            var fileContent = new StreamContent(file.OpenReadStream());
            content.Add(fileContent, "file", file.FileName);

            var response = await _http.PostAsync("api/bank/ocr_extract", content);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ComplianceValidationResultDto>();
        }


        // ================= UPSERT (NO FILE) =================
        public async Task<int> UpsertComplianceAsync( int uploadId,    string accountNo
        )
        {
            var request = new
            {
                UploadId = uploadId,
                DocumentNumber = accountNo
            };

            var response = await _http.PostAsJsonAsync("api/bank/upsert", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Upsert failed: {error}");
            }

            var result = await response.Content.ReadFromJsonAsync<UploadResponseDto>();

            return result?.UploadId ?? 0;
        }

        // ================= SAVE BANK =================
        public async Task SaveAsync(BankSaveRequestDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/bank/save", dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Save failed: {error}");
            }
        }
    }
}