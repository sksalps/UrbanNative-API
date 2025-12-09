using Microsoft.AspNetCore.Mvc;
using UrbanNative.Api.Models;
using UrbanNative.Api.Services;
using UrbanNative.Domain.Entities;

namespace UrbanNative.Api.Controllers
{
    [ApiController]
    [Route("api/vendor")]
    public class VendorController : ControllerBase
    {
        private readonly IVendorService _vendorService;
        private readonly IMessageService _messageService;

        public VendorController(IVendorService vendorService, IMessageService messageService)
        {
            _vendorService = vendorService;
            _messageService = messageService;
        }

        // ===========================
        // 1️⃣ REGISTER VENDOR
        // ===========================
        [HttpPost("register")]
        public async Task<IActionResult> RegisterVendor([FromBody] Vendor vendor)
        {
            var vendorId = await _vendorService.RegisterVendorAsync(vendor);

            if (vendorId == -1)
            {
                return Ok(new
                {
                    Success = false,
                    Message = await _messageService.GetMessageAsync("VENDOR_MOBILE_EXISTS")
                });
            }

            return Ok(new
            {
                Success = true,
                VendorID = vendorId,
                Message = await _messageService.GetMessageAsync("VENDOR_REGISTER_SUCCESS")
            });
        }

        // ===========================
        // 2️⃣ LOGIN VENDOR
        // ===========================

        [HttpPost("login")]
        public async Task<IActionResult> LoginVendor([FromBody] VendorLoginDto request)
        {
            var vendor = await _vendorService.LoginVendorAsync(request.Mobile);

            if (vendor == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = await _messageService.GetMessageAsync("VENDOR_NOT_FOUND")
                });
            }

            return Ok(new
            {
                Success = true,
                Vendor = vendor,
                Message = await _messageService.GetMessageAsync("VENDOR_LOGIN_SUCCESS")
            });
        }

        // ===========================
        // 3️⃣ APPROVE VENDOR (Admin)
        // ===========================
        [HttpPut("approve/{vendorId}")]
        public async Task<IActionResult> ApproveVendor(int vendorId, [FromQuery] int adminId = 1)
        {
            bool result = await _vendorService.ApproveVendorAsync(vendorId, adminId);

            return Ok(new
            {
                Success = result,
                Message = await _messageService.GetMessageAsync("VENDOR_APPROVED")
            });
        }

        // ===========================
        // 4️⃣ REJECT VENDOR (Admin)
        // ===========================
        [HttpPut("reject/{vendorId}")]
        public async Task<IActionResult> RejectVendor(int vendorId, [FromQuery] string reason)
        {
            bool result = await _vendorService.RejectVendorAsync(vendorId, reason, rejectedBy: 1);

            return Ok(new
            {
                Success = result,
                Message = await _messageService.GetMessageAsync("VENDOR_REJECTED")
            });
        }

        // ===========================
        // 5️⃣ UPDATE VENDOR PROFILE
        // ===========================
        [HttpPut("update")]
        public async Task<IActionResult> UpdateVendor([FromBody] Vendor vendor)
        {
            bool result = await _vendorService.UpdateVendorAsync(vendor);

            return Ok(new
            {
                Success = result,
                Message = await _messageService.GetMessageAsync("VENDOR_UPDATED")
            });
        }

        // ===========================
        // 6️⃣ GET VENDOR DETAILS
        // ===========================
        [HttpGet("{vendorId}")]
        public async Task<IActionResult> GetVendor(int vendorId)
        {
            var vendor = await _vendorService.GetVendorByIdAsync(vendorId);

            if (vendor == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = await _messageService.GetMessageAsync("VENDOR_NOT_FOUND")
                });
            }

            return Ok(new
            {
                Success = true,
                Vendor = vendor
            });
        }
    }
}