using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.DTOs.AdminVariant;
using UrbanNative.Application.Interfaces;

namespace UrbanNative.Api.Controllers
{
    [ApiController]
    [Route("api/admin/variant-values")]
    [Authorize] // Admin-only
    public class AdminVariantValuesController : ControllerBase
    {
        private readonly IAdminVariantRepository _variantRepository;

        public AdminVariantValuesController(IAdminVariantRepository variantRepository)
        {
            _variantRepository = variantRepository;
        }

        // =========================
        // GET: Values by Variant
        // =========================
        [HttpGet("by-variant/{variantId:int}")]
        public async Task<IActionResult> GetValuesByVariant(int variantId)
        {
            var values = await _variantRepository.GetVariantValuesAsync(variantId);
            return Ok(values);
        }

        // =========================
        // POST: Add Variant Value
        // =========================

        [HttpPost]
        public async Task<IActionResult> CreateVariantValue(
    [FromBody] CreateVariantValueDto dto)
        {
            try
            {
                await _variantRepository.CreateVariantValueAsync(dto);
                return Ok(new { message = "Variant value added successfully." });
            }
            catch (SqlException ex)
            {
                // Duplicate / business validation errors from SP
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                // Unexpected errors
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // =========================
        // PATCH: Activate / Inactivate Variant Value
        // =========================
        [HttpPatch("{variantValueId:int}/toggle")]
        public async Task<IActionResult> ToggleVariantValueStatus(int variantValueId)
        {
            await _variantRepository.ToggleVariantValueStatusAsync(variantValueId);
            return Ok(new { message = "Variant value status updated." });
        }
    }
}