using System.Net.Http.Json;
using System.Net.Http.Headers;
using UrbanNative.Vendors.Services.Interfaces;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Shared.SharedDTOs;

 namespace UrbanNative.Vendors.Services
{
    public class BankService : IBankService
    {
        private readonly HttpClient _http;

        public BankService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public async Task<ComplianceValidationResultDto> ExtractOnlyAsync(IFormFile file,string complianceName)
        {
            var content = new MultipartFormDataContent();

            content.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);
            content.Add(new StringContent(complianceName), "complianceName");

            var response = await _http.PostAsync("api/bank/ocr_extract", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API ERROR: {response.StatusCode} - {error}");
            }

            return await response.Content.ReadFromJsonAsync<ComplianceValidationResultDto>();
        }



        // ================= SAVE BANK =================
        public async Task SaveFullAsync(IFormFile file, BankFormModel dto)
        {
            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(dto.AccountHolderName ?? ""), "AccountHolderName");
            content.Add(new StringContent(dto.AccountNo ?? ""), "AccountNo");
            content.Add(new StringContent(dto.IFSCCode ?? ""), "IFSCCode");
            content.Add(new StringContent(dto.CityName ?? ""), "CityName");

            var res = await _http.PostAsync("api/bank/save", content);

            if (!res.IsSuccessStatusCode)
            {
                var err = await res.Content.ReadAsStringAsync();
                throw new Exception(err);
            }
        }
    }
}