using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrbanNative.Application.Interfaces;
using UrbanNative.Domain.Entities;

[ApiController]
[Route("api/admin/gst")]
[Authorize]
public class AdminGSTController : ControllerBase
{
    private readonly IAdminGSTRepository _repository;

    public AdminGSTController(IAdminGSTRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        decimal? gstPercentage,
        bool? isActive)
    {
        var data = await _repository.GetGSTAsync(gstPercentage, isActive);
        return Ok(data);
    }



    [HttpPost("{gstId}/toggle")]
    public async Task<IActionResult> Toggle(int gstId)
    {
        int adminId = int.Parse(
            User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value
        );

        var result = await _repository.ToggleGSTAsync(gstId, adminId);

        if (!result)
            return BadRequest("Unable to toggle GST");

        return Ok();
    }



}