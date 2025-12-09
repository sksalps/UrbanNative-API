using Microsoft.AspNetCore.Mvc;
using UrbanNative.Api.Services;

namespace UrbanNative.Api.Controllers
{
    [ApiController]
    [Route("api/categoryvariant")]
    public class CategoryVariantController : ControllerBase
    {
        private readonly ICategoryVariantService _service;
        private readonly IMessageService _messages;

        public CategoryVariantController(
            ICategoryVariantService service,
            IMessageService messages)
        {
            _service = service;
            _messages = messages;
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignVariant(int categoryId, int variantId, int adminId)
        {
            var result = await _service.AssignVariantToCategoryAsync(categoryId, variantId, adminId);

            if (result == -1)
                return Ok(new { Success = false, Message = "Already assigned" });

            return Ok(new { Success = true, Message = "Variant assigned to category" });
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveVariant(int categoryId, int variantId)
        {
            bool result = await _service.RemoveVariantFromCategoryAsync(categoryId, variantId);

            return Ok(new
            {
                Success = result,
                Message = "Variant removed from category"
            });
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var list = await _service.GetVariantsByCategoryAsync(categoryId);
            return Ok(list);
        }
    }
}
