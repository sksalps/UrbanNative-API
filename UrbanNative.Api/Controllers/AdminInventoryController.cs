using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.Interfaces;

namespace UrbanNative.Api.Controllers
{
    [ApiController]
    [Route("api/admin/inventory")]
    [Authorize] // Admin authentication (same pattern as GST, Products)
    public class AdminInventoryController : ControllerBase
    {
        private readonly IAdminInventoryRepository _inventoryRepository;

        public AdminInventoryController(IAdminInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        // =================================================
        // GET: Inventory List (Admin Inventory Main Page)
        // =================================================
        [HttpGet]
        public async Task<IActionResult> GetInventory(
            [FromQuery] string? search,
            [FromQuery] int? categoryId,
            [FromQuery] bool? lowStockOnly,
            [FromQuery] bool? isActive)
        {
            var items = await _inventoryRepository.GetInventoryAsync(
                search,
                categoryId,
                lowStockOnly,
                isActive
            );

            return Ok(items);
        }

        // =================================================
        // GET: Inventory Product Summary (Detail Header)
        // =================================================
        [HttpGet("{productId:int}/summary")]
        public async Task<IActionResult> GetInventorySummary(int productId)
        {
            var summary = await _inventoryRepository.GetInventorySummaryAsync(productId);

            if (summary == null)
                return NotFound();

            return Ok(summary);
        }

        // =================================================
        // GET: Inventory SKU Drill-Down (Read-Only)
        // =================================================
        [HttpGet("{productId:int}/skus")]
        public async Task<IActionResult> GetInventorySkus(int productId)
        {
            var skus = await _inventoryRepository.GetInventorySkusAsync(productId);

            return Ok(skus);
        }
    }
}
