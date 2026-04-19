using UrbanNative.Application.DTOs.Customers.AuthLogin;

namespace UrbanNative.Customers.Services.Interfaces
{
    public interface ICustomerAuthService
    {
        Task<SendOtpResponseDto> SendOtpAsync(string identifier);
        Task<VerifyOtpResponseDto> VerifyOtpAsync(string identifier, int otp);
    }
}
