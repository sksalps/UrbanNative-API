using System.Net.Http.Json;
using UrbanNative.Application.DTOs.AdminHSN;

namespace UrbanNative.Admin.Services
{
    public class AdminHSNService : IAdminHSNService
    {
        private readonly HttpClient _http;

        public AdminHSNService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public async Task<IEnumerable<AdminHSNListDto>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<IEnumerable<AdminHSNListDto>>(
                "api/admin/hsn"
            ) ?? Enumerable.Empty<AdminHSNListDto>();
        }

        public async Task<IEnumerable<AdminHSNListDto>> GetFilterAsync(string? search,
    int? gstId,
    bool? isActive)
        {
            var query = new List<string>();

            if (!string.IsNullOrWhiteSpace(search))
                query.Add($"search={Uri.EscapeDataString(search)}");

            if (gstId.HasValue)
                query.Add($"gstId={gstId.Value}");

            if (isActive.HasValue)
                query.Add($"isActive={isActive.Value}");

            var url = "api/admin/hsn";

            if (query.Any())
                url += "?" + string.Join("&", query);

            return await _http.GetFromJsonAsync<IEnumerable<AdminHSNListDto>>(url)
                   ?? Enumerable.Empty<AdminHSNListDto>();
        }




        public async Task<AdminHSNDetailDto?> GetByIdAsync(int hsnId)
        {
            return await _http.GetFromJsonAsync<AdminHSNDetailDto>(
                $"api/admin/hsn/{hsnId}"
            );
        }

        public async Task CreateAsync(AdminHSNCreateDto dto)
        {
            await _http.PostAsJsonAsync("api/admin/hsn", dto);
        }

        public async Task UpdateAsync(int hsnId, AdminHSNUpdateDto dto)
        {
            await _http.PutAsJsonAsync($"api/admin/hsn/{hsnId}", dto);
        }

        public async Task ToggleActiveAsync(int hsnId)
        {
            await _http.PostAsync($"api/admin/hsn/{hsnId}/toggle", null);
        }
    }
}