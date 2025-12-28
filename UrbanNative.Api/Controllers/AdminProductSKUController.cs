using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.DTOs.AdminSKU;
using UrbanNative.Application.Interfaces;

//namespace UrbanNative.API.Controllers{ }
    [ApiController]
    [Route("api/admin/products/{productId}/skus")]
[Authorize]

public class AdminProductSKUController : ControllerBase
    {
        private readonly IAdminSkuRepository _repo;

        public AdminProductSKUController(IAdminSkuRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateSkus(int productId,        [FromBody] List<VariantSelectionDto> selections,    [FromQuery] decimal price,
            [FromQuery] int stock,
            [FromQuery] int? returnPolicyId)
        {
            if (selections.Any(s => s.VariantId <= 0 || s.VariantValueIds == null || !s.VariantValueIds.Any()))
            {
                return BadRequest("Invalid variant selection supplied.");
            }

            await _repo.GenerateAndSaveSkusAsync(productId,   selections,   price,stock, returnPolicyId  );


            return Ok(new { message = "SKUs generated successfully." });
        }

        // =========================
        // ADMIN SKU OVERVIEW
        // =========================
        [HttpGet("overview")]
        public async Task<IActionResult> GetOverview()
        {
            var data = await _repo.GetSkuOverviewAsync();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductSkus(int productId)
        {
            var data = await _repo.GetProductSkusAsync(productId);
            return Ok(data);
        }

        [HttpGet("vendor-coverage")]
        public async Task<IActionResult> GetVendorCoverage(int productId,[FromQuery] bool includeInactiveVendors = false)
        {
            var data = await _repo.GetVendorCoverageAsync(
                productId,
                includeInactiveVendors
            );

            return Ok(data);
        }
        // =========================
        // PRODUCT COVERAGE HEADER
        // =========================
        [HttpGet("coverage-header")]
        public async Task<IActionResult> GetCoverageHeader(int productId)
        {
            var data = await _repo.GetCoverageHeaderAsync(productId);

            if (data == null)
                return NotFound();

            return Ok(data);
        }
        // =========================
        // VENDOR COVERAGE DETAIL
        // =========================
        [HttpGet("vendor-coverage/{vendorId}")]
        public async Task<IActionResult> GetVendorCoverageDetail( int productId,      int vendorId)
        {
            var data = await _repo.GetVendorCoverageDetailAsync(
                productId,
                vendorId
            );

            return Ok(data);
        }


    }
