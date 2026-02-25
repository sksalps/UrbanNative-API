using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using UrbanNative.Application.Interfaces.UseCases.CommonCrossDashboard.Compliance;
using UrbanNative.Application.UseCase.CommonCroshDashboard.Compliance;
using UrbanNative.Domain.Entities;

namespace UrbanNative.Api.Controllers.Compliance
{
    [Authorize]
    [ApiController]
    [Route("api/compliance")]
    public class ComplianceController : ControllerBase
    {
        private readonly IComplianceUseCase _complianceUseCase;

        public ComplianceController(IComplianceUseCase useCase)
        {
            _complianceUseCase = useCase;
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
    }
}

