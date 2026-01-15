using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UrbanNative.Application.DTOs;
using UrbanNative.Application.Interfaces;
using UrbanNative.Infrastructure.Security;

namespace UrbanNative.Api.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminAuthController : ControllerBase
    {
        private readonly IAdminRepository _repo;
        private readonly IConfiguration _configuration;

        public AdminAuthController(
            IAdminRepository repo,
            IConfiguration configuration)
        {
            _repo = repo;
            _configuration = configuration;
        }
        /// <summary>
        /// Validate admin credentials. Accepts identifier (username or email) and password.
        /// Returns AdminInfoDto on success (200), 401 on failure, 400 on bad request.
        /// </summary>
        [HttpPost("validate")]
        public async Task<IActionResult> Validate([FromBody] AdminValidateRequest req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.Identifier) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("Identifier and password are required.");

            var admin = await _repo.GetByUsernameOrEmailAsync(req.Identifier.Trim());
            if (admin == null) return Unauthorized();

            var verified = PasswordHelper.VerifyPassword(  
                req.Password,     admin.PasswordHash,          admin.PasswordSalt);

            if (!verified) return Unauthorized();

            var jwtSection = _configuration.GetSection("JwtSettings");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                new Claim(ClaimTypes.Name, admin.Username),
                new Claim(ClaimTypes.Email, admin.Email),
                new Claim(ClaimTypes.Role, admin.Role ?? "Admin") // 🔑 REQUIRED
            };

            var key = new SymmetricSecurityKey(    Encoding.UTF8.GetBytes(jwtSection["Key"]!)            );

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

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new  {
                token = jwt,
                admin = new AdminInfoDto
                {
                    Id = admin.Id,
                    Username = admin.Username,
                    Email = admin.Email,
                    DisplayName = admin.DisplayName,
                    Role = admin.Role ?? "Admin"
                }
            });
        }

    }
}



