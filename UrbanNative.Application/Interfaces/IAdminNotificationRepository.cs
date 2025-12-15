using UrbanNative.Application.DTOs;


namespace UrbanNative.Application.Interfaces
{
    public interface IAdminNotificationRepository
    {
        Task<IEnumerable<AdminNotificationDto>> GetUnreadAsync(int adminId);
        Task<int> GetUnreadCountAsync(int adminId);
        Task MarkAsReadAsync(int id);
        Task CreateAsync(AdminNotificationDto notification);
    }
}

