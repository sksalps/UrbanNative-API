using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanNative.Application.DTOs.AdminHSN;
using UrbanNative.Application.Interfaces;

namespace UrbanNative.Api.Controllers
{
    [ApiController]
    [Route("api/admin/hsn")]
    [Authorize]
    public class AdminHSNController : ControllerBase
    {
        private readonly IAdminHSNRepository _hsnRepository;

        public AdminHSNController(IAdminHSNRepository hsnRepository)
        {
            _hsnRepository = hsnRepository;
        }

        // ========================
        // GET: List all HSNs
        // ========================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _hsnRepository.GetAllAsync();
            return Ok(result);
        }

        // ========================
        // GET: HSN by Id
        // ========================
        [HttpGet("{hsnId:int}")]
        public async Task<IActionResult> GetById(int hsnId)
        {
            var result = await _hsnRepository.GetByIdAsync(hsnId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // ========================
        // POST: Create HSN
        // ========================
        [HttpPost]
        public async Task<IActionResult> Create(AdminHSNCreateDto dto)
        {
            int adminId = int.Parse(
                User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value
            );

            dto.CreatedBy = adminId;

            var hsnId = await _hsnRepository.CreateAsync(dto);
            return Ok(new { HSNId = hsnId });
        }

        // ========================
        // PUT: Update HSN
        // ========================
        [HttpPut("{hsnId:int}")]
        public async Task<IActionResult> Update(int hsnId, AdminHSNUpdateDto dto)
        {
            int adminId = int.Parse(
                User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value
            );

            dto.HSNId = hsnId;
            dto.UpdatedBy = adminId;

            await _hsnRepository.UpdateAsync(dto);
            return Ok();
        }

        // ========================
        // POST: Toggle Active
        // ========================
        [HttpPost("{hsnId:int}/toggle")]
        public async Task<IActionResult> Toggle(int hsnId)
        {
            int adminId = int.Parse(
                User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value
            );

            await _hsnRepository.ToggleActiveAsync(hsnId, adminId);
            return Ok();
        }
    }
}
