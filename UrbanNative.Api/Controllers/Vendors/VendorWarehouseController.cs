using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.Interfaces.UseCases;

namespace UrbanNative.Api.Controllers.Vendors
{
    [ApiController]
    [Route("api/vendors/warehouses")]
    public class VendorWarehouseController : ControllerBase
    {
        private readonly IVendorProductsUseCase _useCase;

        public VendorWarehouseController(IVendorProductsUseCase useCase)
        {
            _useCase = useCase;
        }

        //all active inactive both
        [HttpGet] 
        public async Task<IActionResult> GetAll()
        {
            //int vendorId = GetVendorId();

            return Ok(await _useCase.ExecuteAsync(
                null,
                isActive: null));
        }

        // Create / Edit Product
        [HttpGet("create")]
        public async Task<IActionResult> GetForCreate()
        {
            //int vendorId = GetVendorId();

            return Ok(await _useCase.ExecuteAsync(
                null,
                isActive: true));
        }
        // Product list / history (future)
        [HttpGet("vendor")]
        public async Task<IActionResult> GetVendorCat()
        {
            int vendorId = GetVendorId();

            return Ok(await _useCase.ExecuteAsync(
                vendorId,
                isActive: null));
        }
        private int GetVendorId()
        {
            var vendorIdClaim = User.FindFirst("VendorId")?.Value;
            //vendorIdClaim = "1";
            if (string.IsNullOrWhiteSpace(vendorIdClaim))
                throw new UnauthorizedAccessException("VendorID claim missing");

            return int.Parse(vendorIdClaim);
        }
    }


}
