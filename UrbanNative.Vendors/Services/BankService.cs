using System.Net.Http.Json;
using System.Text.Json;
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

                var errorObj = JsonSerializer.Deserialize<ValidationErrorDto>(error);

                if (errorObj?.Errors != null && errorObj.Errors.Any())
                {
                    var messages = errorObj.Errors
                        .SelectMany(e => e.Value)
                        .ToList();

                    throw new Exception(string.Join(", ", messages));
                    //throw new Exception($"API ERROR: {response.StatusCode} - {error}");
                }

                throw new Exception("Something went wrong");
            }


            return await response.Content.ReadFromJsonAsync<ComplianceValidationResultDto>();
        }

        public async Task<ServiceResult> SaveFullAsync(IFormFile file, BankSaveRequestDto dto)
        {
            using var content = new MultipartFormDataContent();

            
            if (file != null && file.Length > 0)
            {
                var fileContent = new StreamContent(file.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

                content.Add(fileContent, "file", file.FileName);
            }
            content.Add(new StringContent(dto.AccountHolderName ?? ""), "AccountHolderName");
            content.Add(new StringContent(dto.AccountNo ?? ""), "AccountNo");
            content.Add(new StringContent(dto.IFSCCode ?? ""), "IFSCCode");
            content.Add(new StringContent(dto.CityName ?? ""), "CityName");
            content.Add(new StringContent(dto.IsPrimary.ToString() ??"" ), "IsPrimary");
            content.Add(new StringContent(dto.BankName ?? ""), "BankName");
            content.Add(new StringContent(dto.BankID.ToString() ?? ""), "BankID");
            content.Add(new StringContent(dto.AccountType ?? ""), "AccountType");
            content.Add(new StringContent(dto.BranchName ?? ""), "BranchName");
            content.Add(new StringContent(dto.UPIId ?? ""), "UPIId");
            content.Add(new StringContent(dto.CountryName ?? ""), "CountryName");
            content.Add(new StringContent(dto.StateName ?? ""), "StateName");
            content.Add(new StringContent(dto.Pincode ?? ""), "Pincode");

            content.Add(new StringContent(dto.ComplianceName ?? ""), "ComplianceName");

            // 🔥 API CALL
            var res = await _http.PostAsync("api/bank/save", content);

            if (!res.IsSuccessStatusCode)
            {
                var err = await res.Content.ReadAsStringAsync();

                // 🔹 Try to parse validation errors
                var errorObj = JsonSerializer.Deserialize<ValidationErrorDto>(
                    err,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // ✅ Case 1: Validation errors
                if (errorObj?.Errors != null && errorObj.Errors.Any())
                {
                    return new ServiceResult
                    {
                        IsSuccess = false,
                        Errors = errorObj.Errors
                    };
                }

                // ❗ Case 2: System / DB / SP error
                return new ServiceResult
                {
                    IsSuccess = false,
                    Message = err // raw error
                };
            }
            return new ServiceResult { IsSuccess = true };
        }

        public async Task<List<BankListDto>> GetBankListAsync()
        {
            var response = await _http.GetAsync("api/bank/bank-list");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<BankListDto>>();
        }
        public async Task<BankSaveRequestDto> GetBankByIdAsync(int? bankId)
        {
            var res = await _http.GetAsync($"api/bank/getbank/{bankId}");
            res.EnsureSuccessStatusCode();

            return await res.Content.ReadFromJsonAsync<BankSaveRequestDto>();
        }
        public async Task<ServiceResult> SetPrimaryBankAsync(int bankId)
        {
            var res = await _http.PostAsJsonAsync(
                "api/bank/set-primary-bank",bankId // ✅ this sends proper JSON
            );

            var content = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
            {
                return new ServiceResult
                {
                    IsSuccess = false,
                    Message = content
                };
            }

            return new ServiceResult { IsSuccess = true };
        }
        public async Task<ServiceResult> DeleteBankAsync(int bankId)
        {
            var res = await _http.PostAsJsonAsync("api/bank/delete-bank", bankId);
            var content = await res.Content.ReadAsStringAsync();
            if (!res.IsSuccessStatusCode)
            {
                return new ServiceResult { IsSuccess = false, Message = content };
            }
            

            return new ServiceResult { IsSuccess = true,Message=content };
        }
        public class ApiResponse
        {
            public string Message { get; set; }
        }
    }
}