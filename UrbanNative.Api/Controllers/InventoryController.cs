using Microsoft.AspNetCore.Mvc;
using UrbanNative.Api.Services;

namespace UrbanNative.Api.Controllers
{
    [ApiController]
    [Route("api/inventory")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _service;

        public InventoryController(IInventoryService service)
        {
            _service = service;
        }

        // 1️⃣ STOCK IN
        [HttpPost("stock-in")]
        public async Task<IActionResult> StockIn([FromBody] dynamic body)
        {
            int variantSetId = body.variantSetId;
            int qty = body.qty;
            int userId = body.userId;

            var stock = await _service.StockInAsync(variantSetId, qty, userId);
            return Ok(new { Success = true, CurrentStock = stock });
        }

        // 2️⃣ STOCK OUT
        [HttpPost("stock-out")]
        public async Task<IActionResult> StockOut([FromBody] dynamic body)
        {
            int variantSetId = body.variantSetId;
            int qty = body.qty;
            int userId = body.userId;

            var result = await _service.StockOutAsync(variantSetId, qty, userId);

            if (result == -1)
                return Ok(new { Success = false, Message = "Insufficient Stock" });

            return Ok(new { Success = true, CurrentStock = result });
        }

        // 3️⃣ GET LOGS
        [HttpGet("logs/{variantSetId}")]
        public async Task<IActionResult> Logs(int variantSetId)
        {
            var logs = await _service.GetLogsAsync(variantSetId);
            return Ok(logs);
        }

        // 4️⃣ CHECK LOW STOCK
        [HttpGet("low-stock/{variantSetId}")]
        public async Task<IActionResult> CheckLowStock(int variantSetId)
        {
            var result = await _service.CheckLowStockAsync(variantSetId);
            return Ok(result);
        }
    }
}
