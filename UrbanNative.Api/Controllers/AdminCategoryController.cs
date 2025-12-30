using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.DTOs.AdminCategory;
using UrbanNative.Application.Interfaces;

namespace UrbanNative.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/categories")]
    [Authorize] // Admin authentication (same as Vendors & Products)
    public class AdminCategoriesController : ControllerBase
    {
        private readonly IAdminCategoryRepository _categoryRepository;
        private readonly IAdminCategoryHSNRepository _categoryHsnRepository;
        public AdminCategoriesController(IAdminCategoryRepository categoryRepository,
    IAdminCategoryHSNRepository categoryHsnRepository)
        {
            _categoryRepository = categoryRepository;
            _categoryHsnRepository = categoryHsnRepository;
        }


        // =========================
        // Admin – Category Listing
        // =========================
        // GET: api/admin/categories
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryRepository.GetAdminCategoriesAsync();
            return Ok(categories);
        }

        // =========================
        // Admin – Category Details
        // =========================
        // GET: api/admin/categories/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryRepository.GetAdminCategoryByIdAsync(id);

            if (category == null)
                return NotFound(new { message = "Category not found" });

            return Ok(category);
        }

        // =========================
        // Admin – Create Category
        // =========================
        // POST: api/admin/categories
        [HttpPost]
        public async Task<IActionResult> CreateCategory(
            [FromBody] AdminCategorySaveDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CategoryName))
                return BadRequest("CategoryName is required");

            // TODO: Replace with actual AdminId from JWT / Claims
            int adminId = int.Parse(User.FindFirst("AdminId")?.Value ?? "0");

            await _categoryRepository.CreateCategoryAsync(dto, adminId);

            return Ok(new
            {
                message = "Category created successfully"
            });
        }

        // =========================
        // Admin – Update Category
        // =========================
        // PUT: api/admin/categories/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(
            int id,
            [FromBody] AdminCategorySaveDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CategoryName))
                return BadRequest("CategoryName is required");

            // TODO: Replace with actual AdminId from JWT / Claims
            int adminId = int.Parse(User.FindFirst("AdminId")?.Value ?? "0");

            var updated = await _categoryRepository.UpdateCategoryAsync(
                id,
                dto,
                adminId
            );

            if (!updated)
                return BadRequest(new { message = "Unable to update category" });

            return Ok(new
            {
                message = "Category updated successfully"
            });
        }

        // =========================
        // Admin – Activate / Deactivate Category
        // =========================
        // POST: api/admin/categories/activate
 
        [HttpPost("activate")]
        public async Task<IActionResult> ToggleActive([FromBody] int categoryId)
        {
            var (success, message) =
                await _categoryRepository.ToggleCategoryActiveAsync(categoryId);

            if (!success)
                return BadRequest(new { message });

            return Ok(new { message });
        }


        // ========== Category HSN Linking===============//
      
        [HttpGet("{categoryId:int}/hsn")]
        public async Task<IActionResult> GetCategoryHSN(int categoryId)
        {
            var result = await _categoryHsnRepository.GetByCategoryIdAsync(categoryId);

            // Always return valid JSON
            return Ok(result);   // result is DTO or null → both serialize correctly
        }


        [HttpPost("{categoryId:int}/hsn")]
        public async Task<IActionResult> LinkOrUpdateHSN(int categoryId,[FromBody] int hsnId)
        {
            int adminId = int.Parse(
                User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value
            );

            await _categoryHsnRepository.LinkOrUpdateAsync(
                new CategoryHSNLinkDto
                {
                    CategoryId = categoryId,
                    HSNId = hsnId,
                    AdminId = adminId
                }
            );

            return Ok();
        }

        [HttpDelete("{categoryId:int}/hsn")]
        public async Task<IActionResult> RemoveHSN(int categoryId)
        {
            int adminId = int.Parse(
                User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value
            );

            await _categoryHsnRepository.RemoveAsync(categoryId, adminId);
            return Ok();
        }

    }
}
