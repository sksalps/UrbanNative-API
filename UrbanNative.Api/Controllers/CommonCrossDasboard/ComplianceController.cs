using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrbanNative.Infrastructure.Services;
using UrbanNative.Application.Interfaces.UseCases.CommonCrossDashboard.Compliance;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Domain.Entities;

namespace UrbanNative.Api.Controllers.Compliance
{
    [Authorize]
    [ApiController]
    [Route("api/compliance")]
    public class ComplianceController : ControllerBase
    {
        private readonly IComplianceUseCase _complianceUseCase;
        private readonly OcrService _ocrService;
        
        public ComplianceController(IComplianceUseCase complianceUseCase,OcrService ocrService)
        {
            _complianceUseCase = complianceUseCase;
            _ocrService = ocrService;
        }

        // 🔐 Resolve Entity Context


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

    // 🔝 Dashboard Summary
    [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardSummary()
        {
            var (entityType, entityId) = ResolveEntity();

            var result = await _complianceUseCase.GetDashboardSummaryAsync(entityType, entityId);
            return Ok(result);
        }

        // 📊 Category Progress
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategoryProgress()
        {
            var (entityType, entityId) = ResolveEntity();

            var result = await _complianceUseCase.GetCategoryProgressAsync(entityType, entityId);
            return Ok(result);
        }

        // 📄 Documents by Category
        [HttpGet("categories/{groupId}/documents")]
        public async Task<IActionResult> GetDocumentsByCategory(int groupId)
        {
            var (entityType, entityId) = ResolveEntity();

            var result = await _complianceUseCase.GetDocumentsByCategoryAsync(entityType, entityId, groupId);
            return Ok(result);
        }

        // 📚 Document History
        [HttpGet("documents/{complianceId}/history")]
        public async Task<IActionResult> GetDocumentHistory(int complianceId)
        {
            var (entityType, entityId) = ResolveEntity();

            var result = await _complianceUseCase.GetDocumentHistoryAsync(entityType, entityId, complianceId);
            return Ok(result);
        }

        // 🟢 Category Status Banner
        [HttpGet("categories/{groupId}/status")]
        public async Task<IActionResult> GetCategoryStatus(int groupId)
        {
            var (entityType, entityId) = ResolveEntity();

            var result = await _complianceUseCase.GetCategoryStatusAsync(entityType, entityId, groupId);
            return Ok(result);
        }

        [HttpPost("documents/upload")]
        public async Task<IActionResult> UploadDocument(
            [FromForm] int complianceId,
            [FromForm] DateTime? expiryDate,
            [FromForm] string? documentNumber,
            [FromForm] IFormFile file)
        {
            var (entityType, entityId) = ResolveEntity();

            using var stream = file.OpenReadStream();

            var request = new ComplianceUploadRequest
            {
                ComplianceID = complianceId,
                ExpiryDate = expiryDate,
                DocumentNumber = documentNumber
            };
            try
            {
                await _complianceUseCase.UploadDocumentAsync(
                entityType,
                entityId,
                request,
                stream,
                file.FileName);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost("ocr/pan")]
        public async Task<IActionResult> ExtractPan(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File missing");

            var tempPath = Path.GetTempFileName();

            using (var stream = System.IO.File.Create(tempPath))
                await file.CopyToAsync(stream);

            var text = _ocrService.ExtractText(tempPath);
            System.IO.File.Delete(tempPath);

            var pan = OcrParser.ExtractPan(text); 
            return Ok(new { value = pan });
        }
    }
}

