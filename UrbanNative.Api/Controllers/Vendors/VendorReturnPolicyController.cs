using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.Interfaces.UseCases;

namespace UrbanNative.Api.Controllers.Vendors
{
  
        [Route("api/vendors/return-policies")]
        public class VendorReturnPoliciesController : ControllerBase
        {
            private readonly IVendorProductsUseCase _useCase;

            public VendorReturnPoliciesController(IVendorProductsUseCase useCase)
            {
                _useCase = useCase;
            }

            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                return Ok(await _useCase.GetReturnPoliciesAsync(
                    isActive: true));
            }
            // Create / Edit Product
            [HttpGet ("create")]
            public async Task<IActionResult> GetActive()
            {
                return Ok(await _useCase.GetReturnPoliciesAsync(
                    isActive: true));
            }
            // Return Policy used by vendor list / history (future)
            [HttpGet("vendor")]
            public async Task<IActionResult> GetAllForVendor()
            {
                return Ok(await _useCase.GetReturnPoliciesAsync(
                    isActive: null));
            }
        }

    }

