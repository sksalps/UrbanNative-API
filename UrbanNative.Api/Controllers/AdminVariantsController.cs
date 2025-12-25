using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.DTOs.AdminVariant;
using UrbanNative.Application.Interfaces;

namespace UrbanNative.Api.Controllers
{
    [ApiController]
    [Route("api/admin/variants")]
    [Authorize] // Admin-only
    public class AdminVariantsController : ControllerBase
    {
        private readonly IAdminVariantRepository _variantRepository;

        public AdminVariantsController(IAdminVariantRepository variantRepository)
        {
            _variantRepository = variantRepository;
        }

        // =========================
        // GET: Variant Listing
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetVariants(
            [FromQuery] string? search,
            [FromQuery] bool? isActive)
        {
            var result = await _variantRepository.GetVariantsAsync(search, isActive);
            return Ok(result);
        }

        // =========================
        // GET: Variant Details
        // =========================
        [HttpGet("{variantId:int}")]
        public async Task<IActionResult> GetVariantById(int variantId)
        {
            var variant = await _variantRepository.GetVariantByIdAsync(variantId);

            if (variant == null)
                return NotFound();

            return Ok(variant);
        }

        // =========================
        // POST: Create Variant
        // =========================
        
        [HttpPost]
        public async Task<IActionResult> CreateVariant([FromBody] CreateVariantDto dto)
        {
            try
            {
                await _variantRepository.CreateVariantAsync(dto);
                return Ok(new { message = "Variant created successfully." });
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // =========================
        // PUT: Update Variant
        // =========================
        [HttpPut("{variantId:int}")]
        public async Task<IActionResult> UpdateVariant(
            int variantId,
            [FromBody] CreateVariantDto dto)
        {
            await _variantRepository.UpdateVariantAsync(variantId, dto.VariantName);
            return Ok(new { message = "Variant updated successfully." });
        }

        // =========================
        // PATCH: Activate / Inactivate Variant
        // =========================
        [HttpPatch("{variantId:int}/toggle")]
        public async Task<IActionResult> ToggleVariantStatus(int variantId)
        {
            await _variantRepository.ToggleVariantStatusAsync(variantId);
            return Ok(new { message = "Variant status updated." });
        }
    }
}