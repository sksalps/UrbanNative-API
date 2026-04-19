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
    }
}