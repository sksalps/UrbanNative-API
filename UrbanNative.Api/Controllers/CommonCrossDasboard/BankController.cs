using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces.UseCase;
using UrbanNative.Application.Interfaces.UseCases.CommonCrossDashboard.Compliance;

[Authorize]
[ApiController]
[Route("api/bank")]
public class BankController : ControllerBase
{
    private readonly IBankUseCase _useCase;

    public BankController(IBankUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpPost("ocr_extract")]
    public async Task<IActionResult> ExtractOnly(IFormFile file, [FromForm] string complianceName)
    {
        var result = await _useCase.ExtractOnlyAsync(file, complianceName);
        return Ok(result);
    }


    [HttpPost("save")]
    public async Task<IActionResult> Save([FromForm] IFormFile? file,[FromForm] BankSaveRequestDto dto)
    {
        var (entityType, entityId) = ResolveEntity();

        dto.EntityType = entityType;
        dto.EntityID = entityId;

        await _useCase.SaveBankAsync(file, dto);

        return Ok();
    }
    [HttpGet("bank-list")]
    public async Task<IActionResult> GetBankList()
    {
        var (entityType, entityId) = ResolveEntity(); // your existing pattern

        var result = await _useCase.HandleAsync(entityType,entityId);

        return Ok(result);
    }
    [HttpGet("getbank/{bankId}")]
    public async Task<IActionResult> GetBankById(int bankId)
    {
        var (entityType, entityId) = ResolveEntity();
        var result = await _useCase.GetByIdAsync(bankId, entityId, entityType);
        return Ok(result);
    }
    [HttpPost("set-primary-bank")]
    public async Task<IActionResult> SetPrimaryBank([FromBody] int bankId)
    {
        var (entityType, entityId) = ResolveEntity();

        await _useCase.ExecuteSetAsync(bankId, entityId,entityType);

        return Ok(new
        {
            message = "Primary bank updated successfully."
        });
    }
    [HttpPost("delete-bank")]
    public async Task<IActionResult> DeleteBank([FromBody] int bankId)
    {
        var (entityType, entityId) = ResolveEntity();

        await _useCase.ExecuteDeleteAsync(bankId,entityId,entityType);

        return Ok(new { message = "Bank deleted successfully." });
    }
    private (string EntityType, int EntityId) ResolveEntity()
    {
        // ✔ Determine entity type from Role
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrWhiteSpace(role))
            throw new UnauthorizedAccessException("Role claim missing.");

        // ✔ Get entity ID from NameIdentifier (standard)
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(idClaim))
            throw new UnauthorizedAccessException("NameIdentifier claim missing.");

        return (role, int.Parse(idClaim));
    }
    
}