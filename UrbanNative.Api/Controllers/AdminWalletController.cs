using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.Interfaces;

namespace UrbanNative.Api.Controllers
{
    [ApiController]
    [Route("api/admin/wallet")]
    public class AdminWalletController : ControllerBase
    {
        private readonly IAdminWalletRepository _repo;

        public AdminWalletController(IAdminWalletRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("ledger")]
        public async Task<IActionResult> GetLedger(
            string ownerType,
            int ownerId,
            string? walletType,
            string? accHead,
            string? txnType,
            string? sourceType,
            int? sourceId,
            DateTime? from,
            DateTime? to)
        {
            ownerType = ownerType?.ToUpper();
            walletType = walletType?.ToUpper();
            accHead = accHead?.ToUpper();
            txnType = txnType?.ToUpper();
            sourceType = sourceType?.ToUpper();

            var data = await _repo.GetLedgerAsync(
                ownerType,
                ownerId,
                walletType,
                accHead,
                txnType,
                sourceType,
                sourceId,
                from,
                to
            );

            return Ok(data);
        }
        [HttpGet("balance-summary")]
        public async Task<IActionResult> GetBalanceSummary(
    string ownerType,
    int ownerId,
    string? walletType,
    string? accHead,
    string? txnType,
    string? sourceType,
    int? sourceId,
    DateTime? from,
    DateTime? to)
        {
            var data = await _repo.GetBalanceSummaryAsync(
                ownerType,
                ownerId,
                walletType,
                accHead,
                txnType,
                sourceType,
                sourceId,
                from,
                to
            );

            return Ok(data);
        }

    }


}
