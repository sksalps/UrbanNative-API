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

        //all category active inactive ?isActive=true|false of all vendors
        [HttpGet("categories")] 
        public async Task<IActionResult> Categories() //all category active inactive
        {
            //int? vendorId = GetVendorIdOrNull();
            return Ok(await _repo.GetCategoriesAsync(null, null));//vendorId,isActive
        }
        //active categories for creating products
        [HttpGet("categories/create")] 
        public async Task<IActionResult> GetCategoriesForCreate()
        {
            //int? vendorId = GetVendorIdOrNull();
            var data = await _repo.GetCategoriesAsync(
                vendorId: null,
                isActive: true);

            return Ok(data);
        }

        //all categories used by vendor active inactive both for filter
        [HttpGet("categories/vendor")] 
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

        [HttpGet("product/{productId:int}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            int? vendorId = GetVendorIdOrNull();

            var result = await _repo.GetProductByIdAsync(vendorId, productId);

            if (result == null)
                return NotFound();

            return Ok(result);
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

            int? vendorId =  GetVendorIdOrNull();
            var data = await _repo.SearchSkusAsync(vendorId, productId, q);
            return Ok(data);
        }

        //=============================
        // Warehouse Fetching common use case
        //============================
        
        //all active inactive both for all vendors
        [HttpGet("warehouses")]
        public async Task<IActionResult> GetWarehouses()
        {
            //int vendorId = GetVendorId();

            return Ok(await _repo.GetWarehousesAsync(null,isActive: null));
        }
        //Get all Active warehouse of a vendor for create or add inventory in active warehouses only
        [HttpGet("warehouses/vendoractive")]
        public async Task<IActionResult> GetVendorActiveWarehouseAsync()
        {
            int? vendorId = GetVendorIdOrNull();

            return Ok(await _repo.GetWarehousesAsync(vendorId, isActive: true));
        }
        //  Fetch Only active warehouses for creating/editing products  
        [HttpGet("warehouses/create")]
        public async Task<IActionResult> GetActiveWarehouses()
        {
            //int vendorId = GetVendorId();

            return Ok(await _repo.GetWarehousesAsync(null,isActive: true));
        }
        // A vendor Warehouse linked with Product list / history (future) active inactive both
        [HttpGet("warehouses/vendor")]
        public async Task<IActionResult> GetVendorWarehouses()
        {
            int? vendorId = GetVendorIdOrNull();

            return Ok(await _repo.GetWarehousesAsync(vendorId, isActive: null));
        }

        // Get Warehouse by ID
        [HttpGet("warehouse/{warehouseId}")]
        public async Task<IActionResult> GetWarehouseById(int warehouseId)
        {
            var data = await _repo.GetWarehouseByIdAsync(warehouseId);
            if (data == null)
                return NotFound();

            return Ok(data);
        }

        // =====================================================
        // Helper
        // =====================================================
        private int? GetVendorIdOrNull()
        {
            var vendorIdClaim =  User.FindFirst("VendorId")?.Value;
            //var vendorIdClaim = "1";
            return string.IsNullOrWhiteSpace(vendorIdClaim)
                ? null
                : int.Parse(vendorIdClaim);
        }
    }
}


