using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.DTOs.Vendors.Wallet;
using UrbanNative.Application.Interfaces.UseCases.Wallet;
using UrbanNative.Domain.Entities;

namespace UrbanNative.Api.Controllers.Vendors
{

        [ApiController]
        [Route("api/vendors/wallet")]
        public class VendorWalletController : ControllerBase
        {
            private readonly IVendorWalletUseCase _useCase;

            public VendorWalletController(IVendorWalletUseCase useCase)
            {
                _useCase = useCase;
            }

        [HttpGet("ledger")]
        public async Task<IActionResult> GetLedger([FromQuery] WalletLedgerFilterDto filter)
        {
            // 🔐 Inject VendorId from claims
            filter.VendorId = GetVendorId(); // extension method

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
