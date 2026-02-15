using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.DTOs.Vendors.Logistics;
using UrbanNative.Application.Interfaces.UseCase.Logistics;
using UrbanNative.Domain.Entities;

[ApiController]
[Route("api/vendor/logistics")]
[Authorize]
public class VendorLogisticsController : ControllerBase
{
    private readonly IVendorLogisticsUseCase _useCase;

    public VendorLogisticsController(IVendorLogisticsUseCase useCase)
    {
        _useCase = useCase;
    }

    /* ================= GRID-1 ================= */

    [HttpGet("orders")]
    public async Task<IActionResult> GetOrders(
    [FromQuery] bool showCompleted = false,
    [FromQuery] string? searchText = null)
    {
        int vendorId = GetVendorId();

        var filter = new VendorLogisticsFilterDto
        {
            ShowCompleted = showCompleted,
            SearchText = searchText
        };

        var result = await _useCase.GetOrdersAsync(vendorId, filter);
        return Ok(result);
    }


    /* ================= GRID-2 ================= */

    [HttpGet("items")]
    public async Task<IActionResult> GetOrderItems(
        [FromQuery] int orderId)
    {
        int vendorId = GetVendorId();

        var result = await _useCase.GetOrderItemsAsync(vendorId, orderId);
        return Ok(result);
    }
    /* ================= ORDER SUMMARY ================= */
    [HttpGet("summary")]
    public async Task<IActionResult> GetOrderSummary([FromQuery] int orderId)
    {
        int vendorId = GetVendorId();

        var result = await _useCase.GetOrderSummaryAsync(vendorId, orderId);
        return Ok(result);
    }
    /* ================= Item Warehouse ================= */

    [HttpGet("warehouse")]
    public async Task<IActionResult> GetItemWarehouse(
        [FromQuery] int orderId)
    {
        int vendorId = GetVendorId();

        var result = await _useCase.GetItemWarehouseAsync(vendorId, orderId);
        return Ok(result);
    }



    /* ================= GRID-3 ================= */

    [HttpGet("shipments")]
    public async Task<IActionResult> GetShipments([FromQuery] int? orderId,[FromQuery] bool showCompleted = false)
    {
        int vendorId = GetVendorId();

        var result = await _useCase.GetShipmentsAsync(
            vendorId,
            orderId,
            showCompleted);

        return Ok(result);
    }
    /* ================= CREATE SHIPMENT ================= */

    [HttpPost("shipments")]
    public async Task<IActionResult> CreateShipment([FromBody] CreateVendorShipmentDto dto)
    {
        int vendorId = GetVendorId();

        var shipmentId = await _useCase.CreateShipmentAsync(vendorId, dto);
        return Ok(new { ShipmentID = shipmentId });
    }
    /* ================= UPDATE SHIPMENT ================= */

    [HttpPost("shipments/update-status")]
    public async Task<IActionResult> UpdateShipmentStatus([FromBody] UpdateShipmentStatusDto dto)
    {
        int vendorId = GetVendorId();

        await _useCase.UpdateShipmentStatusAsync(vendorId, dto);
        return Ok();
    }

    /* ================= AUTOCOMPLETE ================= */

    [HttpGet("suggestions")]
    public async Task<IActionResult> GetSuggestions(
        [FromQuery] string term)
    {
        int vendorId = GetVendorId();

        var result = await _useCase.GetFilterSuggestionsAsync(vendorId, term);
        return Ok(result);
    }

    /* ================= VENDOR CONTEXT ================= */

    private int GetVendorId()
    {
        var vendorIdClaim = User.FindFirst("VendorId")?.Value;

        if (string.IsNullOrWhiteSpace(vendorIdClaim))
            throw new UnauthorizedAccessException("VendorID claim missing");

        return int.Parse(vendorIdClaim);
    }
}
