using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrbanNative.Application.DTOs.CommonCrossDashboard;
using UrbanNative.Application.DTOs.CommonCrossDashboard.Compliance;
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
        public async Task<IActionResult> List(string type)
        {
            var (entityType, entityId) = ResolveEntity();
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

        [HttpGet("country")]
        public async Task<IActionResult> GetCountries([FromQuery] int? countryId)
        {
            var data = await _useCase.GetCountriesAsync(countryId);
            return Ok(data);
        }

        [HttpGet("state")]
        public async Task<IActionResult> GetStates(
            [FromQuery] int? countryId,
            [FromQuery] int? stateId)
        {
            var data = await _useCase.GetStatesAsync(countryId, stateId);
            return Ok(data);
        }

        [HttpGet("city")]
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
