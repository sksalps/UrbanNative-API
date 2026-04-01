using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
using UrbanNative.Application.Interfaces.CommonCrossDashboard.Compliance;
using UrbanNative.Application.Interfaces.UseCases.CommonCrossDashboard.Compliance;
using UrbanNative.Domain.Entities;


namespace UrbanNative.Api.Controllers.Compliance
{
    [Authorize]
    [ApiController]
    [Route("api/compliance")]
    public class ComplianceController : ControllerBase
    {
        private readonly IComplianceUseCase _complianceUseCase;
        private readonly IComplianceValidateUseCase _complianceValidateUseCase;
        private readonly IOcrService _ocrService;
        
        public ComplianceController(IComplianceUseCase complianceUseCase,IComplianceValidateUseCase complianceValidateUseCase, IOcrService ocrService)
        {
            _complianceUseCase = complianceUseCase;
            _complianceValidateUseCase = complianceValidateUseCase;
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
            try
            {
                var (entityType, entityId) = ResolveEntity();

                var result = await _complianceUseCase.GetDashboardSummaryAsync(entityType, entityId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString()); // 🔥 IMPORTANT
            }
        }

        // 📊 Category Progress
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategoryProgress()
        {
            var (entityType, entityId) = ResolveEntity();

            var result = await _complianceUseCase.GetCategoryProgressAsync(entityType, entityId);
            return Ok(result);
        }
        // 📊 Category/Group Grid
        [HttpGet("group")]
        public async Task<IActionResult> GetCategoryGroup()
        {
            var (entityType, entityId) = ResolveEntity();

            var result = await _complianceUseCase.GetCategoryGridAsync(entityType, entityId);
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
            [FromForm] int uploadId,
            [FromForm] DateTime? expiryDate,
            [FromForm] string? documentNumber,
            [FromForm] IFormFile file)
        {
            var (entityType, entityId) = ResolveEntity();

            using var stream = file.OpenReadStream();

            var request = new ComplianceUploadRequest
            {
                ComplianceID = complianceId,
                UploadID=uploadId,
                ExpiryDate = expiryDate,
                DocumentNumber = documentNumber
                
            };
            try
            {
                await _complianceUseCase.UploadDocumentAsync(entityType,entityId,request,file);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost("validate-compliance")]
        public async Task<IActionResult> ValidateCompliance([FromForm] IFormFile File,[FromForm] int ComplianceId,[FromForm] string Regex)
        {
            if (File == null || File.Length == 0)
                return BadRequest("File missing");

            var result = await _complianceValidateUseCase.ExecuteAsync(
                File.OpenReadStream(),
                File.FileName,
                File.Length,
                ComplianceId);

            return Ok(result);
        }
        //============Use this for Menu, Compliance=>Documents List==================//
        
        // Get uploaded compliance documents (Vendor/Admin/Customer/Sathi)
 
        [HttpGet("documents_list")]
        public async Task<IActionResult> GetDocumentsList( )
        {
            var (entityType, entityId) = ResolveEntity();
            

            var result = await _complianceUseCase.GetUploadedDocumentsAsync(entityType, entityId);

            return Ok(result);
        }
        [HttpDelete("deletedoc")]
        public async Task<IActionResult> DeleteDocument(int uploadId)
        {
            var (entityType, entityId) = ResolveEntity();

            await _complianceUseCase.DeleteDocumentAsync(uploadId, entityType, entityId);

            return Ok(new { message = "Document deleted successfully" });
        }
    }
}

