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

            var skus = await _useCase
                .ExecuteAsync(vendorId, productId, warehouseId);

            return Ok(skus);
        }



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
