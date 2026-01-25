using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.DTOs.Vendors.Products;
using UrbanNative.Application.Interfaces.UseCases;

namespace UrbanNative.Api.Controllers.Vendors
{
    [Authorize(Roles = "Vendor")]
    [ApiController]
    [Route("api/vendors/productSKU")]
    public class VendorSkuController : ControllerBase
    {
        private readonly IVendorSkuUseCase _useCase;

        public VendorSkuController(IVendorSkuUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("{productId}/skus")]
        public async Task<IActionResult> GetSkus(int productId)
        {
            var vendorId = GetVendorId();
            return Ok(await _useCase.GetGridAsync(productId, vendorId));
        }

        [HttpPost("{productId}/skus")]
        public async Task<IActionResult> SaveSkus(int productId, List<VendorSkuSaveDto> skus)
        {
            var vendorId = GetVendorId();
            await _useCase.SaveAsync(vendorId, skus);
            return Ok();
        }

        [HttpGet("{productId}/sku-header")]
        public async Task<ActionResult<VendorSkuHeaderDto>> GetSkuHeader(int productId)
        {
            var vendorId = GetVendorId(); // for future validation if needed

            var header = await _useCase.GetHeaderAsync(productId);

            return Ok(header);
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
