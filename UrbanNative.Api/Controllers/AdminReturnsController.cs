using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.Interfaces;

namespace UrbanNative.Api.Controllers
{
    [ApiController]
    [Route("api/admin/returns")]
    [Authorize]   // Same security model as Orders / Products / Vendors
    public class AdminReturnsController : ControllerBase
    {
        private readonly IAdminReturnsRepository _returnsRepository;

        public AdminReturnsController(IAdminReturnsRepository returnsRepository)
        {
            _returnsRepository = returnsRepository;
        }

        // ============================
        // 1️⃣ Returns Listing (Grid)
        // ============================
        [HttpGet]
        public async Task<IActionResult> GetReturns(
            [FromQuery] string? status,
            [FromQuery] int? vendorId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var result = await _returnsRepository.GetReturnsAsync(
                status, vendorId, fromDate, toDate);

            return Ok(result);
        }

        // ============================
        // 2️⃣ Return Details Page
        // ============================
        [HttpGet("{returnId}")]
        public async Task<IActionResult> GetReturnDetails(int returnId)
        {
            var result = await _returnsRepository.GetReturnDetailsAsync(returnId);

            if (result == null)
                return NotFound(new { message = "Return not found" });

            return Ok(result);
        }

        // ============================
        // 3️⃣ Approve / Reject Return
        // ============================
        [HttpPost("{returnId}/decision")]
        public async Task<IActionResult> ApproveReject(
            int returnId,
            [FromBody] ReturnDecisionRequest request)
        {
            var adminId = int.Parse(User.FindFirst("UserId")!.Value);

            var success = await _returnsRepository.ApproveRejectAsync(
                returnId,
                adminId,
                request.IsApproved,
                request.AdminComment,
                request.RemarkText);

            if (!success)
                return BadRequest(new { message = "Unable to update return" });

            return Ok(new { message = "Return updated successfully" });
        }

        // ============================
        // 4️⃣ Create Return Shipment
        // ============================
        [HttpPost("{returnId}/shipment")]
        public async Task<IActionResult> CreateReturnShipment(
            int returnId,
            [FromBody] CreateReturnShipmentRequest request)
        {
            var success = await _returnsRepository.CreateReturnShipmentAsync(
                returnId,
                request.CourierName,
                request.TrackingNumber,
                request.PickupAddress,
                request.DeliveryAddress);

            if (!success)
                return BadRequest(new { message = "Unable to create return shipment" });

            return Ok(new { message = "Return shipment created" });
        }
    }

    // ============================
    // Request Models
    // ============================

    public class ReturnDecisionRequest
    {
        public bool IsApproved { get; set; }
        public string AdminComment { get; set; } = null!;
        public string RemarkText { get; set; } = null!;
    }

    public class CreateReturnShipmentRequest
    {
        public string CourierName { get; set; } = null!;
        public string TrackingNumber { get; set; } = null!;
        public string PickupAddress { get; set; } = null!;
        public string DeliveryAddress { get; set; } = null!;
    }
}
