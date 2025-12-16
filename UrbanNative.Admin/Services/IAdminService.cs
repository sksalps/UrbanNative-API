using UrbanNative.Application.DTOs;

namespace UrbanNative.Application.Interfaces
{
    public interface IAdminService
    {
        // 🔐 JWT-based login
        Task<AdminLoginResponseDto?> ValidateAdminAsync(
            string identifier,
            string password
        );

        // Dashboard
        Task<int> GetVendorsCountAsync();
        Task<int> GetPendingProductsCountAsync();
        Task<int> GetLowStockCountAsync();
        Task<int> GetUsersCountAsync();

        Task<int> GetUnreadNotificationsCountAsync(int adminId);
        Task<List<AdminNotificationDto>> GetUnreadNotificationsAsync(int adminId);
    }
}
