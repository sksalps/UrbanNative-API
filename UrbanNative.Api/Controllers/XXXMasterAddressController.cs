using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.DTOs.Address;
using UrbanNative.Application.Interfaces;

namespace UrbanNative.Api.Controllers
{
    [ApiController]
    [Route("api/address1")]
    [Authorize] // same auth as Vendor/User/Admin
    public class XXXMasterAddressController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;

        public XXXMasterAddressController(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        // =========================
        // Save Address
        // =========================
        [HttpPost("save")]
        public async Task<IActionResult> SaveAddress([FromBody] AddressCreateDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid address data.");

            var addressId = await _addressRepository.SaveAddressAsync(dto);

            return Ok(new
            {
                AddressID = addressId,
                Message = "Address saved successfully"
            });
        }

        // =========================
        // Get Addresses by Entity
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetAddresses(
            [FromQuery] string entityType,
            [FromQuery] int entityId)
        {
            if (string.IsNullOrWhiteSpace(entityType) || entityId <= 0)
                return BadRequest("Invalid entity parameters.");

            var addresses = await _addressRepository
                .GetAddressesAsync(entityType, entityId);

            return Ok(addresses);
        }

        // =========================
        // Deactivate Address
        // =========================
        [HttpPost("deactivate/{addressId:int}")]
        public async Task<IActionResult> DeactivateAddress(int addressId)
        {
            if (addressId <= 0)
                return BadRequest("Invalid Address ID.");

            await _addressRepository.DeactivateAddressAsync(addressId);

            return Ok(new
            {
                Message = "Address deactivated successfully"
            });
        }
    }
}
