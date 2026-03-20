using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces.UseCase;
using UrbanNative.Application.Interfaces.UseCases.CommonCrossDashboard.Compliance;

[ApiController]
[Route("api/bank")]
public class BankController : ControllerBase
{
    private readonly IBankUseCase _useCase;

    public BankController(IBankUseCase useCase)
    {
        _useCase = useCase;
    }

    // ================= VALIDATE + UPLOAD =================

    // ================= UPSERT (NO FILE) =================
    [HttpPost("upsert")]
    public async Task<IActionResult> Upsert([FromBody] BankUpsertRequestDto dto)
    {
        var (entityType, entityId) = ResolveEntity();

        var uploadId = await _useCase.UpsertAsync(
            dto.UploadId,
            dto.DocumentNumber,
            entityType,
            entityId
        );

        return Ok(new { UploadId = uploadId });
    }

    [HttpPost("ocr_extract")]
    public async Task<IActionResult> ExtractOnly(IFormFile file)
    {
        var result = await _useCase.ExtractOnlyAsync(file);
        return Ok(result);
    }
    // ================= SAVE BANK =================
    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] BankSaveRequestDto dto)
    {

        var (entityType, entityId) = ResolveEntity();
        dto.EntityType = entityType;
        dto.EntityID = entityId;
        await _useCase.SaveBankAsync(dto);

        return Ok();
    }

    // ================= HELPERS =================
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