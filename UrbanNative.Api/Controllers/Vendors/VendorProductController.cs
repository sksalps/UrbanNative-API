using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.Interfaces.UseCases;

namespace UrbanNative.Api.Controllers.Vendors
{
    [ApiController]
    [Route("api/vendors/products")]
    public class VendorProductsController : ControllerBase
    {
        private readonly IVendorProductsUseCase _useCase;

        public VendorProductsController(            IVendorProductsUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            string? q,
            int? categoryId,
            int? hsnId)
        {
            int vendorId = GetVendorId()/* from auth */;

            var result = await _useCase.ExecuteAsync(
                vendorId,
                q,
                categoryId,
                hsnId
            );

            return Ok(result);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> Categories()
        {
            int vendorId = GetVendorId()/* from auth */;
            return Ok(await _useCase.GetVendorCategoriesAsync(vendorId));
        }

        [HttpGet("hsn")]
        public async Task<IActionResult> Hsn()
        {
            return Ok(await _useCase.GetVendorHsnListAsync());
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
