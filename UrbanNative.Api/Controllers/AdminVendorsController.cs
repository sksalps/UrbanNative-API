using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.DTOs.AdminVendor;
using UrbanNative.Application.Interfaces;
using UrbanNative.Infrastructure.Services;

namespace UrbanNative.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/vendors")]
    [Authorize] // Admin authentication (same as Products)
    public class AdminVendorsController : ControllerBase
    {
        private readonly IAdminVendorRepository _vendorRepository;

        public AdminVendorsController(IAdminVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        // =========================
        // Admin – Vendor Listing
        // =========================
        // GET: api/admin/vendors
        [HttpGet]
        public async Task<IActionResult> GetVendors(
            [FromQuery] string? search,
            [FromQuery] string? approvalStatus,
            [FromQuery] bool? isActive)
        {
            var vendors = await _vendorRepository.GetAdminVendorsAsync(
                search,
                approvalStatus,
                isActive
            );

            return Ok(vendors);
        }

        // =========================
        // Admin – Vendor Details
        // =========================
        // GET: api/admin/vendors/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetVendorById(int id)
        {
            var vendor = await _vendorRepository.GetAdminVendorByIdAsync(id);

            if (vendor == null)
                return NotFound(new { message = "Vendor not found" });

            return Ok(vendor);
        }
        [HttpGet("{id:int}/referral")]
        public async Task<IActionResult> GetReferral(int id)
        {
            try
            {
                var data = await _vendorRepository.GetVendorReferralAsync(id);
                return Ok(data); // null is OK
            }
            catch (Exception ex)
            {
                // optional: log ex
                return Ok(null); // never break VendorDetails UI
            }
        }


        [HttpGet("{id:int}/media")]
        public async Task<IActionResult> GetMedia(int id)
        {

            try
            {
                var media = await _vendorRepository.GetVendorMediaAsync(id);
                return Ok(media);
            }
            catch (Exception ex)
            {
                // optional: log ex
                return Ok(null); // never break VendorDetails UI
            }
        }

        // =========================
        // Admin – Approve / Reject Vendor
        // =========================
        // POST: api/admin/vendors/approval
        [HttpPost("approval")]
        public async Task<IActionResult> UpdateApproval(
            [FromBody] AdminVendorApprovalDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ApprovalStatus))
                return BadRequest("ApprovalStatus is required");

            // Optional: enforce reason on rejection
            if (dto.ApprovalStatus == "Rejected" &&
                string.IsNullOrWhiteSpace(dto.Reason))
            {
                return BadRequest("Rejection reason is required");
            }

            // TODO: Replace with actual AdminId from JWT / Claims
            int adminId = int.Parse(User.FindFirst("AdminId")?.Value ?? "0");

            await _vendorRepository.UpdateVendorApprovalAsync(
                dto.VendorID,
                adminId,
                dto.ApprovalStatus,
                dto.Reason
            );

            return Ok(new
            {
                message = $"Vendor {dto.ApprovalStatus.ToLower()} successfully"
            });
        }

        // =========================
        // Admin – Activate / Deactivate Vendor
        // =========================
        // POST: api/admin/vendors/activate

        [HttpPost("activate")]
        public async Task<IActionResult> ToggleActive([FromBody] int vendorId)
        {
            var result = await _vendorRepository.ToggleVendorActiveAsync(vendorId);

            if (!result)
                return BadRequest(new { message = "Unable to update vendor status" });

            return Ok(new { message = "Vendor status updated successfully" });
        }

    }
}
