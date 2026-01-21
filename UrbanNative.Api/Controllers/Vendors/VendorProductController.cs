using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.Interfaces.UseCases;

namespace UrbanNative.Api.Controllers.Vendors
{
    [ApiController]
    [Route("api/vendors/products")]
    public class VendorProductsController : ControllerBase
    {
        private readonly IVendorProductsUseCase _useCase;

        public VendorProductsController(IVendorProductsUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            int vendorId = GetVendorId() /* get from auth context */;
            var result = await _useCase.ExecuteAsync(vendorId);
            return Ok(result);
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
