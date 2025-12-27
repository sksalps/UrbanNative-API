using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.DTOs.AdminSKU;
using UrbanNative.Application.Interfaces;

namespace UrbanNative.API.Controllers
{
    [ApiController]
    [Route("api/admin/products/{productId}/skus")]
    public class AdminProductSKUController : ControllerBase
    {
        private readonly IAdminSkuRepository _repo;

        public AdminProductSKUController(IAdminSkuRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateSkus(
            int productId,
            [FromBody] List<VariantSelectionDto> selections,
            [FromQuery] decimal price,
            [FromQuery] int stock,
            [FromQuery] int? returnPolicyId)
        {
            await _repo.GenerateAndSaveSkusAsync(
                productId,
                selections,
                price,
                stock,
                returnPolicyId
            );

            return Ok(new { message = "SKUs generated successfully." });
        }
    }
}
