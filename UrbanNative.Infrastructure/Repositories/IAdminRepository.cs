using System.Threading.Tasks;

namespace UrbanNative.Infrastructure.Repositories
{
    public interface IAdminRepository
    {
        Task<AdminUser?> GetByUsernameOrEmailAsync(string identifier);
    }

    public class AdminUser
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? DisplayName { get; set; } = string.Empty;
        public string Role { get; set; } = "Admin";
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
        public bool IsActive { get; set; } = true;
    }
}
