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

        public AdminCategoriesController(IAdminCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
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

    }
}
