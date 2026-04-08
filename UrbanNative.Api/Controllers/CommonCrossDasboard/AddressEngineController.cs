using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrbanNative.Application.DTOs.CommonCrossDashboard;
using UrbanNative.Application.Interfaces.UseCases.CommonCrossDashboard;

namespace UrbanNative.Api.Controllers.CommonCrossDasboard
{
    
    [Authorize]
    [ApiController]
    [Route("api/address")]
    public class VendorAddressController : ControllerBase
    {
        private readonly IAddressEngineUseCase _useCase;

        public VendorAddressController(IAddressEngineUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("list")]
        public async Task<IActionResult> List(string type)
        {
            var (entityType, entityId) = ResolveEntity();
            return Ok(await _useCase.GetListAsync(entityId, entityType, type));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var (entityType, entityId) = ResolveEntity();
            return Ok(await _useCase.GetByIdAsync(id,entityType,entityId));
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save(AddressSaveDto dto)
        {
            var (entityType, entityId) = ResolveEntity();
            var id = await _useCase.SaveAsync(dto, entityId,entityType);
            return Ok(id);
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
}
