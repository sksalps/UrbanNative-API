using UrbanNative.Application.DTOs;

namespace UrbanNative.Application.DTOs
{
    public class AdminLoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public AdminInfoDto Admin { get; set; } = new();
    }
}
