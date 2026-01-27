using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.Interfaces.UseCases;

namespace UrbanNative.Api.Controllers.Vendors
{
    [ApiController]
    //[Route("api/vendors/categories")]
    [Route("api/vendors/skufilter")]
    public class VendorSkuFilterController : ControllerBase
    {
        private readonly IVendorProductsUseCase _useCase;

        public VendorSkuFilterController(IVendorProductsUseCase useCase)
        {
            _useCase = useCase;
        }

    
        [HttpGet]
        public async Task<IActionResult> Categories() //all active inactive
        {
            int vendorId = GetVendorId()/* from auth */;
            return Ok(await _useCase.GetVendorCategoriesAsync(null,null));
        }
       
        [HttpGet("create")]
        public async Task<IActionResult> GetCategoriesForCreate()
        {
            int vendorId = GetVendorId()/* from auth */;
            var data = await _useCase.GetVendorCategoriesAsync(
                vendorId: null,
                isActive: true);

            return Ok(data);
        }


        [HttpGet("filter")]
        public async Task<IActionResult> GetCategoriesForFilter()
        {
            int vendorId = GetVendorId();

            var data = await _useCase.GetVendorCategoriesAsync(vendorId: vendorId, isActive: null);

            return Ok(data);
        }

        //get hsn by category id
        [HttpGet("{categoryId}/hsn")]
        public async Task<IActionResult> GetCategoryHsn(int categoryId)
        {
            var data = await _useCase.GetHsnByCategoryAsync(categoryId);
            return Ok(data);
        }

        //Get VendorId from JWT token claims
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
