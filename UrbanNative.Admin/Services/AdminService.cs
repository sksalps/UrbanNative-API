using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UrbanNative.Api.Models;   // <-- DTO from API project

namespace UrbanNative.Admin.Services
{
    public class AdminService : IAdminService
    {
        private readonly HttpClient _http;

        public AdminService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        // Correct return type: AdminInfoDto
        public async Task<AdminInfoDto?> ValidateAdminAsync(string identifier, string password)
        {
            if (string.IsNullOrWhiteSpace(identifier) || string.IsNullOrWhiteSpace(password))
                return null;

            var payload = new { Identifier = identifier, Password = password };

            HttpResponseMessage res;
            try
            {
                res = await _http.PostAsJsonAsync("/api/admin/validate", payload);
            }
            catch
            {
                return null;
            }

            if (!res.IsSuccessStatusCode)
                return null;

            try
            {
                return await res.Content.ReadFromJsonAsync<AdminInfoDto>();
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> GetVendorsCountAsync()
        {
            var r = await _http.GetFromJsonAsync<int?>("/api/admin/stats/vendors-count");
            return r ?? 0;
        }

        public async Task<int> GetPendingProductsCountAsync()
        {
            var r = await _http.GetFromJsonAsync<int?>("/api/admin/stats/pending-products");
            return r ?? 0;
        }

        public async Task<int> GetLowStockCountAsync()
        {
            var r = await _http.GetFromJsonAsync<int?>("/api/admin/stats/low-stock-count");
            return r ?? 0;
        }

        public async Task<int> GetUsersCountAsync()
        {
            var r = await _http.GetFromJsonAsync<int?>("/api/admin/stats/users-count");
            return r ?? 0;
        }
    }
}
