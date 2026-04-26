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
    public class AddressEngineController : ControllerBase
    {
        private readonly IAddressEngineUseCase _useCase;

        public AddressEngineController(IAddressEngineUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("list")]
        [AllowAnonymous]
        public async Task<IActionResult> List(string type,int? EntityId=null)
        {
            var entityType = string.Empty;
            var entityId = 0;
            if (type == "NATIVE")
            {
                entityType = type;
                entityId = EntityId ?? 0;
            }
            else
            {
                (entityType, entityId) = ResolveEntity();
            }
            return Ok(await _useCase.GetListAsync(entityId, entityType, type));
        }

        [HttpGet("listall")]
        public async Task<IActionResult> ListAll()
        {
            var (entityType, entityId) = ResolveEntity();
            return Ok(await _useCase.GetListAllAsync(entityId, entityType));
        }
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteWarehouse([FromBody] int id)
        {
            var (entityType, entityId) = ResolveEntity();

            try
            {
                await _useCase.DeleteAddressAsync(id, entityId,entityType);
                return Ok("Deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("set-primary-address")]
        public async Task<IActionResult> SetPrimaryAddress([FromBody] int addressId)
        {
            var (entityType, entityId) = ResolveEntity();

            await _useCase.ExecuteSetPrimaryAsync(addressId, entityId, entityType);

            return Ok(new
            {
                message = "Primary Address updated successfully."
            });
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id, [FromQuery] string? EntityType = null, [FromQuery] int? EntityId = null)
        {
            var entityType = string.Empty;
            var entityId = 0;
            if (EntityType == "NATIVE")
            {
                entityType = EntityType;
                entityId = EntityId ?? 0;
            }
            else
            {
                (entityType, entityId) = ResolveEntity();
            }
            //var (entityType, entityId) = ResolveEntity();
            return Ok(await _useCase.GetByIdAsync(id,entityType,entityId));
        }

        [HttpPost("save")] // 🔥 existing endpoint for JWT save
        public async Task<IActionResult> Save(AddressSaveDto dto)
        {

            if (dto.EntityType == null)
            {
                var (entityType, entityId) = ResolveEntity();
                dto.EntityType = entityType;
                dto.EntityID = entityId;
            }
            else { return BadRequest("Invalid EntityType for anonymous save.");        }

            var result = await _useCase.SaveAsync(dto);

            if (result == null)
            {
                return StatusCode(500, new ServiceResultDto
                {
                    IsSuccess = false,
                    Message = "Failed to save address"
                });
            }

            return Ok(result);
        }

        [HttpPost("savenative")] // 🔥 new endpoint for anonymous save   
        [AllowAnonymous]
        public async Task<IActionResult> SaveNative([FromBody] AddressSaveDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new ServiceResultDto
                {
                    IsSuccess = false,
                    Message = "Invalid request"
                });
            }

            // 🔥 Normalize EntityType
            var entityType = dto.EntityType?.Trim().ToUpper();

            // 🔥 Resolve if not provided
            if (string.IsNullOrEmpty(entityType))
            {
                var (resolvedType, resolvedId) = ResolveEntity();

                dto.EntityType = resolvedType;
                dto.EntityID = resolvedId;
            }
            else if (entityType != "NATIVE")
            {
                return BadRequest(new ServiceResultDto
                {
                    IsSuccess = false,
                    Message = "Invalid EntityType. Must be 'NATIVE' for anonymous save."
                });
            }

            // 🔥 Call use case
            var result = await _useCase.SaveAsync(dto);

            if (result == null)
            {
                return StatusCode(500, new ServiceResultDto
                {
                    IsSuccess = false,
                    Message = "Failed to save address"
                });
            }

            return Ok(result);
        }

        [HttpGet("country")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCountries([FromQuery] int? countryId)
        {
            var data = await _useCase.GetCountriesAsync(countryId);
            return Ok(data);
        }

        [HttpGet("state")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStates(
            [FromQuery] int? countryId,
            [FromQuery] int? stateId)
        {
            var data = await _useCase.GetStatesAsync(countryId, stateId);
            return Ok(data);
        }

        [HttpGet("city")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCities(
            [FromQuery] int? stateId,
            [FromQuery] int? cityId)
        {
            var data = await _useCase.GetCitiesAsync(stateId, cityId);
            return Ok(data);
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
