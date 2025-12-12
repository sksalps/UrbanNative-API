namespace UrbanNative.Admin.Models
{
    public class AdminInfo
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        // Add other admin fields you need (DisplayName, Roles, etc.)
    }
}