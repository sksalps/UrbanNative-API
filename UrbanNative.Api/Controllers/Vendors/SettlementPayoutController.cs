// Controllers/Vendors/SettlementPayoutController.cs
using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.Interfaces.UseCases.Wallet;

namespace UrbanNative.Api.Controllers.Vendors;

[ApiController]
[Route("api/vendors/payout")]
public class SettlementPayoutController : ControllerBase
{
    private readonly ISettlementPayoutUseCase _useCase;

    public SettlementPayoutController(ISettlementPayoutUseCase useCase)
    {
        _useCase = useCase;
    }
     // replace with logged-in vendor id
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(string filterType = "CURRENT_FY", DateTime? fromDate = null, DateTime? toDate = null)
    {
        int vendorId = GetVendorId();
        return Ok(await _useCase.GetSummaryAsync(vendorId, filterType, fromDate, toDate));
    }
    [HttpGet("settlements")]
    public async Task<IActionResult> GetSettlements( string filterType = "CURRENT_FY", DateTime? fromDate = null, DateTime? toDate = null)
    { 
        int vendorId = GetVendorId();
        return Ok(await _useCase.GetSettlementsAsync(vendorId, filterType, fromDate, toDate));
    }
    [HttpGet("payments")]
    public async Task<IActionResult> GetPayments(long settlementId)
    { 
        int vendorId = GetVendorId();
        return Ok(await _useCase.GetPaymentsAsync(settlementId, vendorId));
    }
    private int GetVendorId()
    {
        var vendorIdClaim = User.FindFirst("VendorId")?.Value;
        //var vendorIdClaim = "1";
        if (string.IsNullOrWhiteSpace(vendorIdClaim))
            throw new UnauthorizedAccessException("VendorID claim missing");

        return int.Parse(vendorIdClaim);
    }
}