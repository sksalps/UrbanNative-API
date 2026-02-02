using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.DTOs.Vendors.Inventory;
using UrbanNative.Application.Interfaces.UseCases.VendorInventoryAdd;

namespace UrbanNative.Api.Controllers.Vendors
{
    [Authorize]
    [ApiController]
    [Route("api/vendor/inventoryadd")]
    public class VendorAddInventoryController : ControllerBase
    {
        private readonly IAddInventoryInUseCase _useCase;

        public VendorAddInventoryController(
            IAddInventoryInUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpPost("product/in")]
        public async Task<IActionResult> AddProductInventoryIn([FromBody] ProductInventoryInRequestDto request)
        {
            int vendorId = GetVendorId(); // existing helper

            var result = await _useCase.ExecuteAsync(vendorId, request);

            return Ok(result);
        }

        [HttpGet("product/{productId:int}/skus")]
        public async Task<IActionResult> GetProductSkusForInventory(int productId, [FromQuery] int warehouseId)
        {
            int vendorId = GetVendorId();

            var skus = await _useCase.ExecuteAsync(vendorId, productId, warehouseId);

            return Ok(skus);
        }
        // ======================================================
        // ADD INVENTORY TO SINGLE SKU
        // ======================================================
        [HttpPost("sku/add")]
        public async Task<IActionResult> AddSkuInventory(
            [FromBody] AddSkuInventoryApiRequest request)
        {
            if (request.Quantity <= 0)
                return BadRequest("Quantity must be greater than zero.");

            var vendorId = GetVendorId(); // existing extension

            var result = await _useCase.ExecuteAsyncAddStockSKU(
                vendorId,
                request.SkuId,
                request.WarehouseId,
                request.Quantity,
                request.Remarks);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        // ======================================================
        // SKU STOCK SUMMARY
        // ======================================================
        [HttpGet("sku/{skuId:int}/stock")]
        public async Task<IActionResult> GetSkuStockSummary(
            int skuId,
            [FromQuery] int warehouseId)
        {
            var vendorId = GetVendorId();

            var summary = await _useCase.ExecuteAsyncSummary(
                vendorId,
                skuId,
                warehouseId);

            return Ok(summary);
        }


        // ===========End Single SKU Add==========

        // Placeholder for vendor ID retrieval logic
        private int GetVendorId()
        {
            var vendorIdClaim = User.FindFirst("VendorId")?.Value;
            //var vendorIdClaim = "1";
            if (string.IsNullOrWhiteSpace(vendorIdClaim))
                throw new UnauthorizedAccessException("VendorID claim missing");

            return int.Parse(vendorIdClaim);
        }
    }

}
