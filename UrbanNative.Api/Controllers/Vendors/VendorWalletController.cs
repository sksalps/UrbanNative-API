using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.DTOs.Vendors.Wallet;
using UrbanNative.Application.Interfaces.UseCases.Wallet;
using UrbanNative.Domain.Entities;

namespace UrbanNative.Api.Controllers.Vendors
{
        [Authorize]
        [ApiController]
        [Route("api/vendors/wallet")]
        public class VendorWalletController : ControllerBase
        {
            private readonly IVendorWalletUseCase _useCase;

            public VendorWalletController(IVendorWalletUseCase useCase)
            {
                _useCase = useCase;
            }

        [HttpPost("ledger")]
        public async Task<IActionResult> GetLedger([FromBody] WalletLedgerFilterDto filter)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            filter.VendorId = GetVendorId(); // injected

            var result = await _useCase.ExecuteAsync(filter);
            return Ok(result);
        }


        [HttpGet("account-heads")]
            public async Task<IActionResult> GetAccountHeads()
            {
                int VendorId = GetVendorId(); // extension method
            var result = await _useCase.GetAccountHeadsAsync(VendorId);
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
}
