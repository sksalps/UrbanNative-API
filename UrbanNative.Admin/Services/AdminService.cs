using UrbanNative.Application.DTOs;
using UrbanNative.Application.Interfaces;

namespace UrbanNative.Admin.Services
{
    public class AdminService : IAdminService
    {
        private readonly HttpClient _http;

        public AdminService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        // 🔐 JWT-based authentication
        public async Task<AdminLoginResponseDto?> ValidateAdminAsync(
            string identifier,
            string password)
        {
            var payload = new AdminValidateRequest
            {
                Identifier = identifier,
                Password = password
            };

            var res = await _http.PostAsJsonAsync("/api/admin/validate", payload);
            if (!res.IsSuccessStatusCode)
                return null;

            return await res.Content.ReadFromJsonAsync<AdminLoginResponseDto>();
        }

        // =====================
        // Dashboard Statistics
        // =====================
        public async Task<int> GetVendorsCountAsync()
        {
            var r = await _http.GetFromJsonAsync<int?>("/api/admin/stats/vendors-count");
            return r ?? 0;
        }

        public async Task<int> GetUnreadNotificationsCountAsync(int adminId)
        {
            return await _http.GetFromJsonAsync<int>(
                $"/api/admin/notifications/count/{adminId}");
        }

        public async Task<List<AdminNotificationDto>> GetUnreadNotificationsAsync(int adminId)
        {
            return await _http.GetFromJsonAsync<List<AdminNotificationDto>>(
                $"/api/admin/notifications/unread/{adminId}");
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
