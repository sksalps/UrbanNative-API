using Microsoft.AspNetCore.Mvc;
using UrbanNative.Infrastructure.Repositories;
using UrbanNative.Infrastructure.Security;
using System.Threading.Tasks;

using UrbanNative.Application.DTOs;
using UrbanNative.Application.Interfaces;


namespace UrbanNative.Api.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminAuthController : ControllerBase
    {
        private readonly IAdminRepository _repo;
        public AdminAuthController(IAdminRepository repo)
        {
            _repo = repo;
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

            var verified = PasswordHelper.VerifyPassword(req.Password, admin.PasswordHash, admin.PasswordSalt);
            if (!verified) return Unauthorized();

            var dto = new AdminInfoDto
            {
                Id = admin.Id,
                Username = admin.Username,
                Email = admin.Email,
                DisplayName = admin.DisplayName,
                Role = admin.Role ?? "Admin"
            };

            return Ok(dto);
        }
    }
}