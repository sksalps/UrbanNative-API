using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces.UseCases;

namespace UrbanNative.Api.Controllers.Vendors
{
    [ApiController]
    [Route("api/vendor/profile")]
    [Authorize]
    public class VendorProfileController : ControllerBase
    {
        private readonly IVendorProfileUseCase _useCase;

        public VendorProfileController(IVendorProfileUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet]
        public async Task<ActionResult<VendorProfileDto>> GetProfile()
        {
            int vendorId = GetVendorId();

            var profile = await _useCase.GetProfileAsync(vendorId);

            if (profile == null)
                return NotFound();

            return Ok(profile);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] VendorProfileDto dto)
        {
            if (dto == null)
                return BadRequest();

            int vendorId = GetVendorId();

            await _useCase.UpdateProfileAsync(vendorId, dto);

            return Ok(new { message = "Profile updated successfully" });
        }

        // ================= PRIVATE =================

        
        private int GetVendorId()
        {
            var vendorIdClaim = "1";// User.FindFirst("VendorID")?.Value;

            if (string.IsNullOrWhiteSpace(vendorIdClaim))
                throw new UnauthorizedAccessException("VendorID claim missing");

            return int.Parse(vendorIdClaim);
        }

    }
}
