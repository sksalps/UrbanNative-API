using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using UrbanNative.Application.DTOs.AdminVariantSet;
using UrbanNative.Application.Interfaces;

namespace UrbanNative.API.Controllers
{
    [ApiController]
    [Authorize]
    public class AdminVariantSetVariantsController : ControllerBase
    {
        private readonly IAdminVariantSetRepository _repo;

        public AdminVariantSetVariantsController(IAdminVariantSetRepository repo)
        {
            _repo = repo;
        }

        // =========================
        // GET Variants in Set
        // =========================
        [HttpGet("api/admin/variant-sets/{variantSetId:int}/variants")]
        public async Task<IActionResult> GetVariants(int variantSetId)
        {
            var result = await _repo.GetVariantsBySetIdAsync(variantSetId);
            return Ok(result);
        }

        // =========================
        // ADD Variant to Set
        // =========================
        [HttpPost("api/admin/variant-sets/{variantSetId:int}/variants")]
        public async Task<IActionResult> AddVariant(
    int variantSetId,
    [FromBody] AddVariantToSetRequestDto dto)
        {
            try
            {
                await _repo.AddVariantToSetAsync(variantSetId, dto.VariantID);
                return Ok();
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // =========================
        // REMOVE Variant from Set
        // =========================
        [HttpDelete("api/admin/variant-set-variants/{variantSetVariantId:int}")]
        public async Task<IActionResult> RemoveVariant(int variantSetVariantId)
        {
            await _repo.RemoveVariantFromSetAsync(variantSetVariantId);
            return Ok();
        }

        // =========================
        // UPDATE ORDER
        // =========================
        [HttpPut("api/admin/variant-set-variants/{variantSetVariantId:int}/order")]
        public async Task<IActionResult> UpdateOrder(
    int variantSetVariantId,
    [FromBody] UpdateVariantOrderRequestDto dto)
        {
            await _repo.UpdateVariantOrderAsync(variantSetVariantId, dto.SortOrder);
            return Ok();
        }

    }
}