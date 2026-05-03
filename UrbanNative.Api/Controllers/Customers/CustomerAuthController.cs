using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TesseractOCR.Renderers;
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
            //var result = await _repo.SendOtpAsync(req.Identifier.Trim());

            // TEMP return OTP
            return Ok(new
            {
                Status = "OTP_SENT",
                OTP = otp, //Temp: return OTP for testing
                ExpiryAt = DateTime.UtcNow.AddMinutes(5),
                ReferralCode=req.ReferralCode
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

            // 🔐 VERIFY HASHED OTP USING HELPER
            var isValid = PasswordHelper.VerifyPassword(
                req.OTP.ToString(),
                otpRecord.OTPHash,
                otpRecord.OTPSalt
            );

            if (!isValid)
                return Unauthorized("Invalid OTP");

            // ✅ Mark verified
           // await _repo.MarkOtpVerifiedAsync(otpRecord.OTPID);

            // ==========================
            // CHECK USER EXISTENCE
            // ==========================

            var user = await _repo.GetUserAsync(null, req.Identifier);

            if (user != null)
            {
                var token = GenerateJwt(user);

                //API for verify OTP for existing and new user both returns login response with token
                return Ok(new  
                {
                    success = true,
                    Status = "LOGIN",
                    userExist = new UserLoginResponseDto
                    {
                        LoginStatus = "SUCCESS",
                        UserID = user.UserID,
                        UserRandomID = user.UserRandomID,
                        ReferralCode = user.ReferralCode,
                        UserNickName = user.UserNickName,
                        FullName = user.FullName,
                        Mobile = user.Mobile,
                        Email = user.Email,
                        SponsorReferralCode = user.SponsorReferralCode,
                        SponsorName = user.SponsorName,
                        Role = "Customer",

                        Token = token
                    }
                });
            }
            else
            {
                //var temp = await _repo.CreateTempUserAsync(req.Identifier);
                var temp = await _repo.CreateTempUserAsync(req.Identifier, req.ByReferralCode);
                return Ok(new
                {
                    success = true,
                    Status = "NEW_USER",
                    TempID = temp.TempID,
                    TempToken = temp.TempToken,
                    ReferredByUserId = temp.ReferredByUserID,
                    ByReferralCode = req.ByReferralCode
                });
            }
             
        }
        [HttpPost("insert-tempuser")]
        public async Task<IActionResult> InsertTempUser([FromBody] CreateTempUserRequestDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid request");

            var result = await _repo.InsertTempUserAsync(dto);

            if (result == null)
                return BadRequest("Unable to create temp user");

            return Ok(result);
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
            // ==========================
            // CHECK USER EXISTENCE
            // ==========================
            user = await _repo.GetUserAsync(null, req.Identifier);

            if (user != null)
            {
                var token = GenerateJwt(user);


                return Ok(new
                {
                    success = true,
                    Status = "LOGIN",
                    userExist = new UserLoginResponseDto
                    {
                        LoginStatus = "SUCCESS",
                        UserID = user.UserID,
                        UserRandomID = user.UserRandomID,
                        ReferralCode = user.ReferralCode,
                        UserNickName = user.UserNickName,
                        FullName = user.FullName,
                        Mobile = user.Mobile,
                        Email = user.Email,
                        SponsorReferralCode = user.SponsorReferralCode,
                        SponsorName = user.SponsorName,
                        Role = "Customer",

                        Token = token
                    }
                });
            }
            else
            {
                return Unauthorized("Invalid credentials");
            }

            
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto req)
        {
            try
            {
                var result = await _repo.CompleteRegistrationAsync(req);

                if (result == null || result.Status == "ERROR" || result.UserID == 0)
                {
                    return BadRequest(new
                    {
                        Status = "ERROR",
                        Message = "Registration failed"
                    });
                }
                var user = await _repo.GetUserAsync(result.UserID, null);
                if (user == null)
                {
                    return BadRequest(new
                    {
                        Status = "ERROR",
                        Message = "Registration Successful, but user data missing"
                    });
                }
                var token = GenerateJwt(user);
                //new registration also returns same login response with token
                return Ok(new
                {
                    
                    Status = result.Status,
                    Message = "Registration Successful, Login Proceed",
                    userExist = new UserLoginResponseDto
                    {
                        LoginStatus = "SUCCESS",
                        UserID = user.UserID,
                        UserRandomID = user.UserRandomID,
                        ReferralCode = user.ReferralCode,
                        UserNickName = user.UserNickName,
                        FullName = user.FullName,
                        Mobile = user.Mobile,
                        Email = user.Email,
                        SponsorReferralCode = user.SponsorReferralCode,
                        SponsorName = user.SponsorName,
                        Role = "Customer",

                        Token = token
                    }
                });               
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Status = "ERROR",
                    Message = ex.Message   // 🔥 important
                });
            }
        }

        [HttpGet("temp-user")]
        public async Task<IActionResult> GetTempUser(int tempId, string token)
        {
            var temp = await _repo.GetTempUserAsync(tempId, token);

            if (temp == null)
                return NotFound();

            return Ok(temp);
        }

        // ==========================================
        // 🔑 JWT GENERATION
        // ==========================================
        private string GenerateJwt(AuthUserDto user)
        {
            var jwtSection = _configuration.GetSection("JwtSettings");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserID.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                new Claim("UserID", user.UserID.ToString()),
                new Claim("UserRandomID", user.UserRandomID.ToString()),
                new Claim("UserNickName", user.UserNickName ?? ""),
                new Claim(ClaimTypes.Name, user.FullName ?? ""),
                new Claim(ClaimTypes.MobilePhone, user.Mobile ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim("SponsorReferralCode", user.SponsorReferralCode ?? ""),
                new Claim("SponsorName", user.SponsorName ?? ""),
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
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSection["ExpiryMinutes"])),
                signingCredentials: creds
            );
            //user.Token = new JwtSecurityTokenHandler().WriteToken(token);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }

}