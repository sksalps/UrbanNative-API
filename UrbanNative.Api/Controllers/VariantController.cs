using Microsoft.AspNetCore.Mvc;
using UrbanNative.Api.Services;
using UrbanNative.Domain.Entities;

namespace UrbanNative.Api.Controllers
{
    [ApiController]
    [Route("api/variant")]
    public class VariantController : ControllerBase
    {
        private readonly IVariantMasterService _variantMasterService;
        private readonly IVariantValueService _variantValueService;
        private readonly IProductVariantSetService _variantSetService;
        private readonly IProductVariantValuesService _variantSetValueService;
        private readonly ICategoryVariantService _categoryVariantService;
        private readonly IMessageService _messages;

        public VariantController(
            IVariantMasterService variantMasterService,
            IVariantValueService variantValueService,
            IProductVariantSetService variantSetService,
            IProductVariantValuesService variantSetValueService,
            ICategoryVariantService categoryVariantService,
            IMessageService messages)
        {
            _variantMasterService = variantMasterService;
            _variantValueService = variantValueService;
            _variantSetService = variantSetService;
            _variantSetValueService = variantSetValueService;
            _categoryVariantService = categoryVariantService;
            _messages = messages;
        }

        // ------------------------------------------------------------
        // 1️⃣ ADD VARIANT TYPE (Color, Size, Weight)
        // ------------------------------------------------------------
        [HttpPost("type/add")]
        public async Task<IActionResult> AddVariantType([FromBody] dynamic body)
        {
            string variantName = body.variantName;

            var id = await _variantMasterService.AddVariantTypeAsync(variantName);

            return Ok(new
            {
                Success = true,
                VariantID = id,
                Message = "Variant type added successfully"
            });
        }

        // ------------------------------------------------------------
        // 2️⃣ GET ALL VARIANT TYPES
        // ------------------------------------------------------------
        [HttpGet("type/all")]
        public async Task<IActionResult> GetAllTypes()
        {
            var list = await _variantMasterService.GetAllVariantTypesAsync();
            return Ok(list);
        }

        // ------------------------------------------------------------
        // 3️⃣ ADD VARIANT VALUE (Red, XL, 1kg...)
        // ------------------------------------------------------------
        [HttpPost("value/add")]
        public async Task<IActionResult> AddVariantValue([FromBody] dynamic body)
        {
            int variantId = body.variantId;
            string valueName = body.valueName;

            var id = await _variantValueService.AddVariantValueAsync(variantId, valueName);

            return Ok(new
            {
                Success = true,
                ValueID = id,
                Message = "Variant value added successfully"
            });
        }

        // ------------------------------------------------------------
        // 4️⃣ GET VALUES FOR VARIANT TYPE
        // ------------------------------------------------------------
        [HttpGet("values/{variantId}")]
        public async Task<IActionResult> GetValues(int variantId)
        {
            var values = await _variantValueService.GetValuesByVariantAsync(variantId);
            return Ok(values);
        }

        // ------------------------------------------------------------
        // 5️⃣ ADMIN: ASSIGN VARIANT TYPE TO CATEGORY
        // ------------------------------------------------------------
        [HttpPost("category/assign")]
        public async Task<IActionResult> AssignVariantToCategory([FromBody] dynamic body)
        {
            int categoryId = body.categoryId;
            int variantId = body.variantId;
            int adminId = body.adminId;

            var result = await _categoryVariantService.AssignVariantToCategoryAsync(categoryId, variantId, adminId);

            if (result == -1)
                return Ok(new { Success = false, Message = "Already assigned to this category" });

            return Ok(new { Success = true, Message = "Variant assigned to category" });
        }

        // ------------------------------------------------------------
        // 6️⃣ ADMIN: REMOVE VARIANT TYPE FROM CATEGORY
        // ------------------------------------------------------------
        [HttpPost("category/remove")]
        public async Task<IActionResult> RemoveVariantFromCategory([FromBody] dynamic body)
        {
            int categoryId = body.categoryId;
            int variantId = body.variantId;

            var result = await _categoryVariantService.RemoveVariantFromCategoryAsync(categoryId, variantId);

            return Ok(new { Success = result, Message = "Variant removed from category" });
        }

        // ------------------------------------------------------------
        // 7️⃣ GET VARIANTS FOR GIVEN CATEGORY
        // ------------------------------------------------------------
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetCategoryVariants(int categoryId)
        {
            var list = await _categoryVariantService.GetVariantsByCategoryAsync(categoryId);
            return Ok(list);
        }

        // ------------------------------------------------------------
        // 8️⃣ ADD VARIANT SET (SKU)
        // ------------------------------------------------------------
        [HttpPost("set/add")]
        public async Task<IActionResult> AddVariantSet([FromBody] ProductVariantSet set)
        {
            var id = await _variantSetService.AddVariantSetAsync(set);

            return Ok(new
            {
                Success = true,
                VariantSetID = id,
                Message = "Variant set (SKU) created successfully"
            });
        }

        // ------------------------------------------------------------
        // 9️⃣ GET VARIANT SETS FOR PRODUCT
        // ------------------------------------------------------------
        [HttpGet("set/list/{productId}")]
        public async Task<IActionResult> GetVariantSets(int productId)
        {
            var sets = await _variantSetService.GetVariantSetsByProductAsync(productId);
            return Ok(sets);
        }

        // ------------------------------------------------------------
        // 🔟 ASSIGN VARIANT VALUES TO SKU
        // ------------------------------------------------------------
        [HttpPost("set/value/add")]
        public async Task<IActionResult> AddValueToVariantSet([FromBody] dynamic body)
        {
            int variantSetId = body.variantSetId;
            int valueId = body.valueId;

            bool success = await _variantSetValueService.AddValueToVariantSetAsync(variantSetId, valueId);

            return Ok(new
            {
                Success = success,
                Message = "Variant value assigned to set"
            });
        }

        // ------------------------------------------------------------
        // 1️⃣1️⃣ GET ALL VARIANT VALUES FOR SKU
        // ------------------------------------------------------------
        [HttpGet("set/value/{variantSetId}")]
        public async Task<IActionResult> GetValuesForVariantSet(int variantSetId)
        {
            var list = await _variantSetValueService.GetValuesByVariantSetAsync(variantSetId);
            return Ok(list);
        }
    }
}
