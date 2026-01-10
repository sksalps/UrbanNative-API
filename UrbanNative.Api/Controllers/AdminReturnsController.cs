using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrbanNative.Application.Interfaces;

namespace UrbanNative.Api.Controllers
{
    [ApiController]
    [Route("api/admin/returnsorder")]
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
                return NotFound();

            var images = await _returnsRepository.GetReturnImagesAsync(returnId);
            result.Images = images.ToList();

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
            //var adminId = int.Parse(User.FindFirst("UserId")!.Value);
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            //var adminId = int.Parse(User.FindFirst("AdminId")!.Value);


            var success = await _returnsRepository.ApproveRejectAsync(
                returnId,
                adminId,
                request.IsApproved,
                request.AdminComment,
                request.RemarkText);

            if (!success)
                return BadRequest(new { message = "Unable to update return" });

            return Ok(new { message = "Return updated" });
        }

        [HttpPost("{returnId}/shipment")]
        public async Task<IActionResult> CreateReturnShipment(
            int returnId,
            [FromBody] CreateReturnShipmentRequest request)
        {
            var success = await _returnsRepository.CreateReturnShipmentAsync(
                returnId,
                request.LogisticsProviderID,
                request.TrackingNumber);

            if (!success)
                return BadRequest(new { message = "Unable to create return shipment" });

            return Ok(new { message = "Return shipment created" });
        }

        [HttpPost("{returnId}/status")]
        public async Task<IActionResult> UpdateStatus(  int returnId,    [FromBody] ReturnStatusRequest request)
        {
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var success = await _returnsRepository.UpdateStatusAsync(
                returnId,
                adminId,
                request.NewStatus,
                request.AdminComment,
                request.RemarkText);

            if (!success)
                return BadRequest(new { message = "Unable to update return status" });

            return Ok(new { message = "Return status updated" });
        }
        [HttpGet("{returnId}/images")]
        public async Task<IActionResult> GetImages(int returnId)
        {
            var images = await _returnsRepository.GetReturnImagesAsync(returnId);
            return Ok(images);
        }

        [HttpPost("{returnId}/images")]
        public async Task<IActionResult> UploadImage(int returnId, [FromBody] ReturnImageRequest request)
        {
            var success = await _returnsRepository.AddReturnImageAsync(
                returnId,
                request.ImageUrl,
                request.UploadedBy,
                request.IsPrimary);

            if (!success)
                return BadRequest();

            return Ok();
        }

        public class ReturnImageRequest
        {
            public string ImageUrl { get; set; } = null!;
            public string UploadedBy { get; set; } = null!;   // CUSTOMER / ADMIN / SATHI
            public bool IsPrimary { get; set; }
        }


        
        [HttpGet("logisticsproviders")]
        public async Task<IActionResult> GetLogisticsProviders()
        {
            var data = await _returnsRepository.GetLogisticsProvidersAsync();
            return Ok(data);
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
            public int LogisticsProviderID { get; set; }
            public string TrackingNumber { get; set; } = null!;
        }

        public class ReturnStatusRequest
        {
            public string NewStatus { get; set; } = null!;   // APPROVED / REJECTED / NEEDS_INFO
            public string AdminComment { get; set; } = null!;
            public string RemarkText { get; set; } = null!;
        }
    }
}
