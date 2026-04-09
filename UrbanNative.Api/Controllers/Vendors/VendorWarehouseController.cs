using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.Interfaces.UseCases;

namespace UrbanNative.Api.Controllers.Vendors
{
    [Authorize]
    [ApiController]
    [Route("api/vendors/warehouses")]
    public class VendorWarehouseController : ControllerBase
    {
        private readonly IVendorProductsUseCase _useCase;
        private readonly IVendorWarehouseUseCase _useCaseWH;

        public VendorWarehouseController(IVendorProductsUseCase useCase, IVendorWarehouseUseCase useCaseWH)
        {
            _useCase = useCase;
            _useCaseWH = useCaseWH;
        }

        //all active inactive both
        [HttpGet] 
        public async Task<IActionResult> GetAll()
        {
            //int vendorId = GetVendorId();

            return Ok(await _useCase.ExecuteAsync(
                null,
                isActive: null));
        }

        //  Product Only active warehouses for creating editing products
        [HttpGet("create")]
        public async Task<IActionResult> GetForCreate()
        {
            //int vendorId = GetVendorId();

            return Ok(await _useCase.ExecuteAsync(
                null,
                isActive: true));
        }
        // Warehouse linked with vendor Product list / history (future)
        [HttpGet("vendor")]
        public async Task<IActionResult> GetVendorWH()
        {
            int vendorId = GetVendorId();

            return Ok(await _useCase.ExecuteAsync(
                vendorId,
                isActive: null));
        }

        // Get Warehouse by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWarehouse(int id)
        {
            var data = await _useCase.GetWarehouseByIdAsync(id);
            return Ok(data);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetWarehouses()
        {
            var vendorId = GetVendorId(); // your existing auth helper

            var result = await _useCaseWH.GetWarehousesListAsync(vendorId);

            return Ok(result);
        }
        [HttpPost("deletewh")]
        public async Task<IActionResult> DeleteWarehouse([FromBody] int id)
        {
            var vendorId = GetVendorId();

            try
            {
                await _useCaseWH.DeleteWarehouseAsync(id, vendorId);
                return Ok("Deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getwarehouse/{bankId}")]
        public async Task<IActionResult> GetWarehouseById(int warehouseId)
        {
            var (entityType, entityId) = ResolveEntity();
            var result = await _useCaseWH.GetWarehouseByIdAsync(warehouseId, entityId, entityType);
            return Ok(result);
        }

        [HttpPost("savewh")]
        public async Task<IActionResult> SaveWarehouse([FromBody] VendorWarehouseSaveDto dto)
        {
            var (entityType, entityId) = ResolveEntity();

            await _useCaseWH.SaveWarehouseAsync(dto, entityId, entityType);

            return Ok();
        }
        //Get VendorId from JWT token claims
        private int GetVendorId()
        {
            var vendorIdClaim = User.FindFirst("VendorId")?.Value;
            //vendorIdClaim = "1";
            if (string.IsNullOrWhiteSpace(vendorIdClaim))
                throw new UnauthorizedAccessException("VendorID claim missing");

            return int.Parse(vendorIdClaim);
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
