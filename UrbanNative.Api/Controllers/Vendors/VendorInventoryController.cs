using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.Interfaces.UseCases;
using UrbanNative.Application.UseCase;

namespace UrbanNative.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/vendor/inventory")]
    public class VendorInventoryController : ControllerBase
    {
        private readonly IVendorInventoryUseCase _inventoryUseCase;

        public VendorInventoryController(IVendorInventoryUseCase inventoryUseCase)
        {
            _inventoryUseCase = inventoryUseCase;
        }

        // ===============================
        // Inventory Summary (Header Cards)
        // ===============================
        [HttpGet("{skuId}/summary")]
        public async Task<IActionResult> GetInventorySummary(
            int skuId,
            [FromQuery] int addressId,
            [FromQuery] DateTime fromDate,
            [FromQuery] DateTime toDate)
        {
            if (skuId <= 0 || addressId <= 0)
                return BadRequest("Invalid SKU or Warehouse");

            if (fromDate > toDate)
                return BadRequest("Invalid date range");

            //sadfas
            var summary = await _inventoryUseCase.GetInventorySummaryAsync(
                skuId,
                addressId,
                fromDate,
                toDate);

            return Ok(summary);
        }

        // ===============================
        // Inventory Logs (Grid)
        // ===============================
        [HttpGet("{skuId}/logs")]
        public async Task<IActionResult> GetInventoryLogs(
            int skuId,
            [FromQuery] int addressId,
            [FromQuery] DateTime fromDate,
            [FromQuery] DateTime toDate,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            if (skuId <= 0 || addressId <= 0)
                return BadRequest("Invalid SKU or Warehouse");

            if (fromDate > toDate)
                return BadRequest("Invalid date range");

            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 20;

            var logs = await _inventoryUseCase.GetInventoryLogsAsync(
                skuId,
                addressId,
                fromDate,
                toDate,
                page,
                pageSize);

            return Ok(logs);
        }
    }
}
