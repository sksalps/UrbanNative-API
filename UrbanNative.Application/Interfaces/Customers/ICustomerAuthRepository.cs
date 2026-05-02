using UrbanNative.Application.DTOs.Customers.AuthLogin;

namespace UrbanNative.Application.Interfaces.Customers
{
    public interface ICustomerAuthRepository
    {
        Task<dynamic> VerifyOtpAsync(string identifier, int otp);
        Task<dynamic> GetUserForPasswordLoginAsync(string identifier);
        Task InsertOtpAsync(string identifier,string identifierType,byte[] otpHash, byte[] otpSalt,DateTime expiryAt, string ipAddress,string userAgent);
        Task<dynamic> GetAuthUserAsync(int userId);

        Task<OtpRecordDto> GetLatestOtpAsync(string identifier);
        Task MarkOtpVerifiedAsync(int otpId);
        Task<AuthUserDto> GetUserAsync(int? userId, string identifier);
        Task<dynamic> GetUserProfileAsync(int? userId, string identifier);
        Task<dynamic> CreateTempUserAsync(string identifier, string referralCode);
        Task<TempUserDto> GetTempUserAsync(int tempId, string tempToken);
        Task<RegisterResponseDto> CompleteRegistrationAsync(RegisterRequestDto req);
        Task<CreateTempUserResponseDto?> InsertTempUserAsync(CreateTempUserRequestDto dto);
    }
}