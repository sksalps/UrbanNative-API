using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces;
using UrbanNative.Domain.Entities;

[ApiController]
[Route("api/vendor/dashboard")]
[Authorize]
public class VendorDashboardController : ControllerBase
{
    private readonly IVendorDashboardRepository _repo;

    public VendorDashboardController(IVendorDashboardRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<ActionResult<VendorDashboardDto>> Get()
    {
        var vendorId = int.Parse(User.FindFirst("VendorId").Value);

        var data = await _repo.GetVendorDashboardAsync(vendorId);

        return Ok(data);
    }
}
