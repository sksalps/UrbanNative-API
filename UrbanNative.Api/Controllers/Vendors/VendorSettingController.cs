using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces.UseCases;
using UrbanNative.Domain.Entities;

namespace UrbanNative.Api.Controllers.Vendors
{
    [ApiController]
    [Route("api/vendor/settings")]
    [Authorize(Roles = "Vendor")]
    public class VendorSettingsController : ControllerBase
    {
        private readonly IVendorSettingsUseCase _useCase;

        public VendorSettingsController( IVendorSettingsUseCase useCase)
        {
            _useCase = useCase;
        }

        // GET: api/vendor/settings
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            int vendorId = GetVendorId();
            var settings = await _useCase.GetAsync(vendorId);
            return Ok(settings);
        }

        // PUT: api/vendor/settings
        [HttpPut]
        public async Task<IActionResult> Update(     [FromBody] VendorSystemSettingUpdateDto dto)
        {
            if (dto == null)       return BadRequest();

            int vendorId = GetVendorId();

            await _useCase.UpdateAsync(dto, vendorId);
            return Ok();
        }

        [HttpGet("{systemSettingId}/history")]
        public async Task<IActionResult> GetHistory(int systemSettingId)
        {
            int vendorId = GetVendorId();

            var history = await _useCase.GetHistoryAsync(
                vendorId, systemSettingId);

            return Ok(history);
        }


        // ================= PRIVATE =================

        private int GetVendorId()
        {
            var claim = User.FindFirst("VendorId")?.Value;
            //claim="1";
            if (string.IsNullOrWhiteSpace(claim))
                throw new UnauthorizedAccessException("VendorID claim missing");

            return int.Parse(claim);
        }
    }
}
