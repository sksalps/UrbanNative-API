using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.DTOs.Common;
using UrbanNative.Application.UseCases.Vendors;
using UrbanNative.Domain.Entities;

[ApiController]
[Route("api/vendor/account")]
[Authorize(Roles = "Vendor")]
public class VendorAccountController : ControllerBase
{
    private readonly VendorChangePasswordUseCase _useCase;

    public VendorAccountController(  VendorChangePasswordUseCase useCase)
    {
        _useCase = useCase;
    }

    // ================= PUBLIC ACTIONS =================

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(     ChangePasswordRequestDto request)
    {
        try
        {
            var vendorId = GetVendorId();
            var result = await _useCase.ExecuteAsync(vendorId, request.OldPassword, request.NewPassword);
            if (result != ChangePasswordResult.Success)
                return BadRequest(result.ToString());
            //await HttpContext.SignOutAsync();
            return Ok();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }
    




    // ================= PRIVATE =================


    private int GetVendorId()
    {
        var vendorIdClaim = User.FindFirst("VendorId")?.Value;
        //vendorIdClaim = "1";
        if (string.IsNullOrWhiteSpace(vendorIdClaim))
            throw new UnauthorizedAccessException("VendorID claim missing");

        return int.Parse(vendorIdClaim);
    }

}
