using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.Interfaces.CommonCrossDashboard;

namespace UrbanNative.Api.Controllers.Common
{
    [ApiController]
    [Route("api/skufilter")]
    public class SkuFilterController : ControllerBase
    {
        private readonly ISkuFilterRepository _repo;

        public SkuFilterController(ISkuFilterRepository repo)
        {
            _repo = repo;
        }

        // GET /api/skufilter/skus/{skuId}/context
        [HttpGet("skus/{skuId}/context")]
        public async Task<IActionResult> GetSkuContext(int skuId)
        {
            if (skuId <= 0)
                return BadRequest("Invalid SKUId");

            int? vendorId = GetVendorIdOrNull();

            var data = await _repo.GetSkuContextAsync(skuId, vendorId);
            if (data == null)
                return NotFound();

            return Ok(data);
        }


        // =====================================================
        // CATEGORY LOOKUP
        // =====================================================

        //all category active inactive ?isActive=true|false
        [HttpGet("categories")] 
        public async Task<IActionResult> Categories() //all category active inactive
        {
            //int? vendorId = GetVendorIdOrNull();
            return Ok(await _repo.GetCategoriesAsync(null, null));//vendorId,isActive
        }

        [HttpGet("categories/create")] //active categories for creating products
        public async Task<IActionResult> GetCategoriesForCreate()
        {
            //int? vendorId = GetVendorIdOrNull();
            var data = await _repo.GetCategoriesAsync(
                vendorId: null,
                isActive: true);

            return Ok(data);
        }


        [HttpGet("categories/filter")] //all categories used by vendor active inactive both for filter
        public async Task<IActionResult> GetCategoriesForFilter()
        {
            int? vendorId = GetVendorIdOrNull();

            var data = await _repo.GetCategoriesAsync(vendorId: vendorId, isActive: null);

            return Ok(data);
        }

        //get hsn by category id
        [HttpGet("{categoryId}/hsn")]
        public async Task<IActionResult> GetCategoryHsn(int categoryId)
        {
            var data = await _repo.GetHsnByCategoryAsync(categoryId);
            return Ok(data);
        }

        // =====================================================
        // PRODUCT LOOKUP (Category scoped)
        // =====================================================

        // GET /api/skufilter/products?categoryId=12
        [HttpGet("products")]
        public async Task<IActionResult> Products([FromQuery] int categoryId)
        {
            if (categoryId <= 0)
                return BadRequest("categoryId is required");

            int? vendorId = GetVendorIdOrNull();
            var data = await _repo.GetProductsAsync(vendorId, categoryId);
            return Ok(data);
        }

        // =====================================================
        // SKU TYPEAHEAD
        // =====================================================

        // GET /api/skufilter/skus?productId=55&q=whi
        [HttpGet("skus")]
        public async Task<IActionResult> Skus(
            [FromQuery] int productId,
            [FromQuery] string? q)
        {
            if (productId <= 0)
                return BadRequest("productId is required");

            int? vendorId = GetVendorIdOrNull();
            var data = await _repo.SearchSkusAsync(vendorId, productId, q);
            return Ok(data);
        }

        // =====================================================
        // Helper
        // =====================================================
        private int? GetVendorIdOrNull()
        {
            var vendorIdClaim = User.FindFirst("VendorId")?.Value;
            return string.IsNullOrWhiteSpace(vendorIdClaim)
                ? null
                : int.Parse(vendorIdClaim);
        }
    }
}


