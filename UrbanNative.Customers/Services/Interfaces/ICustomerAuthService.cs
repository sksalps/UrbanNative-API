using UrbanNative.Application.DTOs.Customers.AuthLogin;

namespace UrbanNative.Customers.Services.Interfaces
{
    public interface ICustomerAuthService
    {
        Task<SendOtpResponseDto> SendOtpAsync(string identifier);
        Task<VerifyOtpResponseDto> VerifyOtpAsync(string identifier, int otp);
        Task<TempUserDto> GetTempUserAsync(int tempId, string token);
        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto req);
        Task<CreateTempUserResponseDto?> InsertTempUserAsync(CreateTempUserRequestDto dto);
    }
}
