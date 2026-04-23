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

    public async Task InsertOtpAsync(string identifier,
    string identifierType,
    byte[] otpHash,
    byte[] otpSalt,
    DateTime expiryAt,
    string ipAddress,
    string userAgent)
        {
            using var conn = _connectionFactory.CreateConnection();

            await conn.ExecuteAsync(
                "sp_UserLogin_InsertOTP",
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
                "sp_UserLogin_GetUserForPasswordLogin",
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
                "sp_UserLogin_VerifyOTP",
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
                "sp_UserLogin_GetLatestOTP",
                new { Identifier = identifier },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task MarkOtpVerifiedAsync(int otpId)
        {
            using var conn = _connectionFactory.CreateConnection();

            await conn.ExecuteAsync(
                "sp_UserLogin_MarkOTPVerified",
                new { OTPID = otpId },
                commandType: CommandType.StoredProcedure
            );
        }


        public async Task<dynamic> GetAuthUserAsync(int userId)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync(
                "sp_UserLogin_GetAuthUser",
                new
                {
                    UserID = userId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<dynamic> GetUserAsync(int? userId, string identifier)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync(
                "sp_UserLogin_GetUserByUid_Identifier",
                new
                {
                    UserID = userId,
                    Identifier = identifier
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<CreateTempUserResponseDto?> InsertTempUserAsync(CreateTempUserRequestDto dto)
        {
            using var conn = _connectionFactory.CreateConnection();

            var result = await conn.QueryFirstOrDefaultAsync<CreateTempUserResponseDto>(
                "sp_UserRegistration_CreateTempUser",
                new
                {
                    Identifier = dto.Identifier,
                    SessionID = dto.SessionID,
                    ReferredByUserID = dto.ReferredByUserID,
                    UserAgent = dto.UserAgent,
                    IPAddress = dto.IPAddress
                },
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        //On OTP verfication, if user is new we create a temp user record
        //and return tempId and tempToken to client, which will be used for final registration
        public async Task<dynamic> CreateTempUserAsync(string identifier) 
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync(
                "sp_UserRegistration_CreateTempUser",
                new
                {
                    Identifier = identifier
                },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<TempUserDto> GetTempUserAsync(int tempId, string tempToken)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<TempUserDto>(
                "sp_UserRegistration_GetTempUser",
                new
                {
                    TempID = tempId,
                    TempToken = tempToken
                },
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<RegisterResponseDto> CompleteRegistrationAsync(RegisterRequestDto req)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<RegisterResponseDto>(
                "sp_UserRegistration_Complete",
                new
                {
                    TempID = req.TempID,
                    TempToken = req.TempToken,
                    ReferredByUserID = req.ReferredByUserID,
                    Name = req.Name,
                    Mobile = req.Mobile,
                    Email = req.Email,
                    Pincode = req.Pincode,
                    AddressID = req.AddressID
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
