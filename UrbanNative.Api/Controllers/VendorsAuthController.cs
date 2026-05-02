using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces;
using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Security;

namespace UrbanNative.Api.Controllers
{
    [ApiController]
    [Route("api/vendor/auth")]
    public class VendorAuthController : ControllerBase
    {
        private readonly IVendorAuthRepository _repo;
        private readonly IConfiguration _configuration;

        public VendorAuthController(
            IVendorAuthRepository repo,
            IConfiguration configuration)
        {
            _repo = repo;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] VendorLoginRequestDto req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.Identifier) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("Identifier and password are required.");

            var vendor = await _repo.GetVendorForLoginAsync(req.Identifier.Trim());

            if (vendor == null)
                return Unauthorized("Invalid credentials");

            // 🔒 Approval check
            if (!string.Equals(vendor.ApprovalStatus, "APPROVED", StringComparison.OrdinalIgnoreCase))
                return Unauthorized("Vendor not approved");

            // 🔒 Active check
            if (!vendor.IsActive)
                return Unauthorized("Vendor is disabled");

            // 🔐 Password check
            var verified = PasswordHelper.VerifyPassword(
                req.Password,
                vendor.PasswordHash,
                vendor.PasswordSalt
            );
           /* var verified = PasswordHasher.VerifyPassword(
                req.Password, vendor.PasswordHash,    vendor.PasswordSalt      );*/
            //verified=true; // Temporarily bypassing password verification for testing purposes.

            if (!verified)
                return Unauthorized("Invalid credentials");

            var jwt = JWTAuthenticationToken(vendor);

            // ==========================
            // Response
            // ==========================
            return Ok(new VendorLoginResponseDto
            {
                Token = jwt,
                VendorID = vendor.VendorID,
                VendorName = vendor.VendorName,
                BusinessName = vendor.BusinessName,
                ContactPerson=vendor.ContactPerson,
                Mobile = vendor.Mobile
            });
        }

        // ==========================
        // 🔑 JWT Generation
        // ==========================
        private string JWTAuthenticationToken(VendorLoginResultDto vendor) {
            var jwtSection = _configuration.GetSection("JwtSettings");

            var claims = new List<Claim>
            {
                new Claim("VendorId", vendor.VendorID.ToString()),      // 🔥 MUST MATCH API USAGE
                new Claim(ClaimTypes.NameIdentifier, vendor.VendorID.ToString()),
                new Claim(ClaimTypes.Name, vendor.VendorName ?? ""),
                new Claim("BusinessName", vendor.BusinessName ?? ""),
                new Claim("ContactPerson", vendor.ContactPerson ??  ""),
                new Claim(ClaimTypes.Email, vendor.Email ?? ""),
                new Claim(ClaimTypes.MobilePhone, vendor.Mobile ?? ""),
                new Claim(ClaimTypes.Role, "Vendor")

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
