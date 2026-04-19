using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UrbanNative.Application.DTOs.Customers.AuthLogin;
using UrbanNative.Application.Interfaces.Customers;
using UrbanNative.Infrastructure.Security;


namespace UrbanNative.Api.Controllers.Customer
{
    [ApiController]
    [Route("api/customer/auth")]
    public class CustomerAuthController : ControllerBase
    {
        private readonly ICustomerAuthRepository _repo;
        private readonly IConfiguration _configuration;
        
        public CustomerAuthController(
            ICustomerAuthRepository repo,
            IConfiguration configuration)
        {
            _repo = repo;
            _configuration = configuration;
        }

        // ==========================================
        // 📲 SEND OTP (TEMP: returns OTP)
        // ==========================================
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpRequestDto req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.Identifier))
                return BadRequest("Identifier required");

            //var result = await _repo.SendOtpAsync(req.Identifier.Trim());


            var otp = new Random().Next(100000, 999999).ToString();

            // 🔐 Hash + Salt
            var (hash, salt) = PasswordHelper.CreateHash(otp);
            //var httpContext = _httpContextAccessor.HttpContext;

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ipAddress = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            }

            var userAgent = Request.Headers["User-Agent"].ToString();

            await _repo.InsertOtpAsync(
                req.Identifier,
                req.Identifier.Contains("@") ? "EMAIL" : "MOBILE",
                hash,
                salt,
                DateTime.UtcNow.AddMinutes(5),
                ipAddress,
                userAgent
            );

            // TEMP return OTP
            return Ok(new
            {
                Status = "OTP_SENT",
                OTP = otp, //Temp: return OTP for testing
                ExpiryAt = DateTime.UtcNow.AddMinutes(5)
            });

            
        }

        // ==========================================
        // 🔐 VERIFY OTP
        // ==========================================
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequestDto req)
        {
            var otpRecord = await _repo.GetLatestOtpAsync(req.Identifier);

            if (otpRecord == null)
                return Unauthorized("OTP not found");

            if (otpRecord.IsVerified)
                return Unauthorized("OTP already used");

            if (otpRecord.ExpiryAt < DateTime.UtcNow)
                return Unauthorized("OTP expired");

            // 🔐 VERIFY USING HELPER
            var isValid = PasswordHelper.VerifyPassword(
                req.OTP.ToString(),
                otpRecord.OTPHash,
                otpRecord.OTPSalt
            );

            if (!isValid)
                return Unauthorized("Invalid OTP");

            // ✅ Mark verified
            await _repo.MarkOtpVerifiedAsync(otpRecord.OTPID);

            // ==========================
            // CHECK USER EXISTENCE
            /* ==========================
            var user = await _repo.GetUserByIdentifierAsync(req.Identifier);

            if (user != null)
            {
                var token = GenerateJwt(user);

                return Ok(new
                {
                    Status = "SUCCESS",
                    UserID = user.UserID,
                    Token = token
                });
            }
            else
            {
                var temp = await _repo.CreateTempUserAsync(req.Identifier);

                return Ok(new
                {
                    Status = "NEW_USER",
                    TempID = temp.TempID,
                    TempToken = temp.TempToken
                });
            }*/
             return Ok(new
            {
                Status = "SUCCESS"
            });
        }


        // ==========================================
        // 🔐 PASSWORD LOGIN
        // ==========================================
        [HttpPost("login-password")]
        public async Task<IActionResult> LoginWithPassword([FromBody] CustomerPasswordLoginRequestDto req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.Identifier) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("Identifier and password required");

            var user = await _repo.GetUserForPasswordLoginAsync(req.Identifier.Trim());

            if (user == null)
                return Unauthorized("Invalid credentials");

            if (!user.IsActive)
                return Unauthorized("User is disabled");

            var verified = PasswordHelper.VerifyPassword(
                req.Password,
                user.PasswordHash,
                user.PasswordSalt
            );

            if (!verified)
                return Unauthorized("Invalid credentials");

            var token = GenerateJwt(user);

            return Ok(new VerifyOtpResponseDto
            {
                Status = "SUCCESS",
                UserID = user.UserID,
                Token = token
            });
        }

        // ==========================================
        // 🔑 JWT GENERATION
        // ==========================================
        private string GenerateJwt(dynamic user)
        {
            var jwtSection = _configuration.GetSection("JwtSettings");

            var claims = new List<Claim>
            {
                new Claim("UserID", user.UserID.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.Name ?? ""),
                new Claim(ClaimTypes.MobilePhone, user.Mobile ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim("UserRandomID", user.UserRandomID.ToString()),
                new Claim(ClaimTypes.Role, "Customer")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSection["Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(jwtSection["ExpiryMinutes"])
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}