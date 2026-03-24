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
    public async Task<IActionResult> Save(
    [FromForm] IFormFile? file,
    [FromForm] BankSaveRequestDto dto)
    {
        var (entityType, entityId) = ResolveEntity();

        dto.EntityType = entityType;
        dto.EntityID = entityId;

        await _useCase.SaveBankAsync(file, dto);

        return Ok();
    }
    /*
    [HttpPost("save")]
    public async Task<IActionResult> Save([FromForm] IFormFile file,[FromForm] BankSaveFormDto formDto)
    {
        // 🔴 VALIDATION (ONLY FORM FIELDS NOW)
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (entityType, entityId) = ResolveEntity();

        // 🔥 MAP FORM DTO → FULL DTO
        var dto = new BankSaveRequestDto
        {
            AccountHolderName = formDto.AccountHolderName,
            AccountNo = formDto.AccountNo,
            IFSCCode = formDto.IFSCCode,
            CityName = formDto.CityName,

            BankName = formDto.BankName,
            BranchName = formDto.BranchName,
            UPIId = formDto.UPIId,
            StateName = formDto.StateName,
            Pincode = formDto.Pincode,

            // 🔐 SYSTEM FIELDS
            EntityID = entityId,
            EntityType = entityType,
            ComplianceName = "Bank Details",
            IsPrimary = true,
            IsFromCompliance = true
        };

        await _useCase.SaveBankAsync(file, dto);

        return Ok(new { message = "Bank saved successfully" });
    }*/
    // ================= SAVE BANK =================
    /*
    [HttpPost("save")]
    public async Task<IActionResult> Save( [FromForm] IFormFile file, [FromForm] BankSaveRequestDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // 🔥 THIS WAS MISSING
        }

        var (entityType, entityId) = ResolveEntity();
        dto.EntityType = entityType;
        dto.EntityID = entityId;
        await _useCase.SaveBankAsync(file,dto);

        return Ok();
    }
    */
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