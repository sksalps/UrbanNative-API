using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using UrbanNative.Application.DTOs.AdminVariantSet;
using UrbanNative.Application.Interfaces;

namespace UrbanNative.API.Controllers
{
    [ApiController]
    [Authorize]
    public class AdminVariantSetCategoriesController : ControllerBase
    {
        private readonly IAdminVariantSetRepository _repo;

        public AdminVariantSetCategoriesController(IAdminVariantSetRepository repo)
        {
            _repo = repo;
        }

        // =========================
        // GET Assigned Categories
        // =========================
        [HttpGet("api/admin/variant-sets/{variantSetId:int}/categories")]
        public async Task<IActionResult> GetCategories(int variantSetId)
        {
            var result = await _repo.GetCategoriesBySetIdAsync(variantSetId);
            return Ok(result);
        }

        [HttpPost("api/admin/variant-set-variants/{id:int}/move")]
        public async Task<IActionResult> MoveVariant(   int id, [FromBody] MoveVariantRequestDto dto)
        {
            await _repo.MoveVariantAsync(id, dto.Direction);
            return Ok();
        }


        [HttpGet("api/admin/variant-sets/{variantSetId:int}/categories/available")]
        public async Task<IActionResult> GetAvailableCategories(int variantSetId)
        {
            var result = await _repo.GetAvailableCategoriesAsync(variantSetId);
            return Ok(result);
        }


        // =========================
        // ASSIGN Category
        // =========================
        [HttpPost("api/admin/variant-sets/{variantSetId:int}/categories")]
        public async Task<IActionResult> AssignCategory(
    int variantSetId,
    [FromBody] AssignVariantSetCategoryRequestDto dto)
        {
            try
            {
                await _repo.AssignCategoryAsync(variantSetId, dto.CategoryID);
                return Ok();
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // =========================
        // REMOVE Category
        // =========================
        [HttpDelete("api/admin/variant-set-categories/{categoryVariantSetId:int}")]
        public async Task<IActionResult> RemoveCategory(int categoryVariantSetId)
        {
            await _repo.RemoveCategoryAsync(categoryVariantSetId);
            return Ok();
        }
    }
}