using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.DTOs.Vendors.Orders;
using UrbanNative.Application.Interfaces.UseCases;

namespace UrbanNative.Api.Controllers.Vendors
{
    [Authorize]
    [ApiController]
    [Route("api/vendors/orders")]
    public class VendorOrdersController : ControllerBase
    {
        private readonly IVendorOrderUseCase _useCase;

        public VendorOrdersController(IVendorOrderUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetOrders(
            [FromBody] VendorOrderListFilterDto filter)
        {
            int vendorId = GetVendorId();
            filter.VendorID = vendorId;
            var orders = await _useCase.GetOrdersAsync(filter);
            return Ok(orders);
        }

        [HttpPost("summary")]
        public async Task<IActionResult> GetSummary(
            [FromBody] VendorOrderListFilterDto filter)
        {
            int vendorId = GetVendorId();
            filter.VendorID = vendorId;
            var summary = await _useCase.GetSummaryAsync(filter);
            return Ok(summary);
        }

        [HttpGet("sku-product-suggestions")]
        public async Task<IActionResult> GetSkuProductSuggestions(   [FromQuery] string term)
        {
           if (string.IsNullOrWhiteSpace(term))
                return Ok(new List<VendorSkuProductSuggestionDto>());

            int vendorId = GetVendorId();

            var result = await _useCase
                .GetSkuProductSuggestionsAsync(vendorId, term);

            return Ok(result);
        }
        private int GetVendorId()
        {
            var vendorIdClaim = User.FindFirst("VendorId")?.Value;
            //var vendorIdClaim = "1";
            if (string.IsNullOrWhiteSpace(vendorIdClaim))
                throw new UnauthorizedAccessException("VendorID claim missing");

            return  int.Parse(vendorIdClaim);
        }

    }

}
