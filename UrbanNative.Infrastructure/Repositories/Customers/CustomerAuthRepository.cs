using Dapper;
using System.Data;
using UrbanNative.Application.DTOs.Customers.AuthLogin;
using UrbanNative.Application.Interfaces.Customers;
using UrbanNative.Infrastructure.Database;


namespace UrbanNative.Infrastructure.Repositories.Customers
{

    public class CustomerAuthRepository : ICustomerAuthRepository
    {
      

        private readonly SqlConnectionFactory _connectionFactory;

        public CustomerAuthRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task InsertOtpAsync(
    string identifier,
    string identifierType,
    byte[] otpHash,
    byte[] otpSalt,
    DateTime expiryAt,
    string ipAddress,
    string userAgent)
        {
            using var conn = _connectionFactory.CreateConnection();

            await conn.ExecuteAsync(
                "UserLogin_InsertOTP",
                new
                {
                    Identifier = identifier,
                    IdentifierType = identifierType,
                    OTPHash = otpHash,
                    OTPSalt = otpSalt,
                    ExpiryAt = expiryAt,
                    ProcessSource = "CUSTOMER_LOGIN",
                    IPAddress = ipAddress,
                    UserAgent = userAgent
                },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<dynamic> GetUserForPasswordLoginAsync(string identifier)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync(
                "UserLogin_GetUserForPasswordLogin",
                new
                {
                    Identifier = identifier,
                    IdentifierType = identifier.Contains("@") ? "EMAIL" : "MOBILE"
                },
                commandType: CommandType.StoredProcedure
            );
        }
        
        public async Task<dynamic> VerifyOtpAsync(string identifier, int otp)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync(
                "UserLogin_VerifyOTP",
                new
                {
                    Identifier = identifier,
                    OTP = otp
                },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<OtpRecordDto> GetLatestOtpAsync(string identifier)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<OtpRecordDto>(
                "UserLogin_GetLatestOTP",
                new { Identifier = identifier },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task MarkOtpVerifiedAsync(int otpId)
        {
            using var conn = _connectionFactory.CreateConnection();

            await conn.ExecuteAsync(
                "UserLogin_MarkOTPVerified",
                new { OTPID = otpId },
                commandType: CommandType.StoredProcedure
            );
        }


        public async Task<dynamic> GetAuthUserAsync(int userId)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync(
                "UserLogin_GetAuthUser",
                new
                {
                    UserID = userId
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
