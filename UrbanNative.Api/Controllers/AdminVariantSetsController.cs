using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using UrbanNative.Application.DTOs.AdminVariantSet;
using UrbanNative.Application.Interfaces;

namespace UrbanNative.API.Controllers
{
    [ApiController]
    [Route("api/admin/variant-sets")]
    [Authorize]
    public class AdminVariantSetsController : ControllerBase
    {
        private readonly IAdminVariantSetRepository _repo;

        public AdminVariantSetsController(IAdminVariantSetRepository repo)
        {
            _repo = repo;
        }

        // =========================
        // GET : All Variant Sets
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repo.GetAllAsync();
            return Ok(result);
        }

        // =========================
        // GET : Variant Set Details
        // =========================
        [HttpGet("{variantSetId:int}")]
        public async Task<IActionResult> GetById(int variantSetId)
        {
            var result = await _repo.GetByIdAsync(variantSetId);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // =========================
        // CREATE Variant Set
        // =========================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVariantSetDto dto)
        {
            try
            {
                await _repo.CreateAsync(dto.VariantSetName);
                return Ok();
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // =========================
        // UPDATE Variant Set
        // =========================
        [HttpPut("{variantSetId:int}")]
        public async Task<IActionResult> Update(
            int variantSetId,
            [FromBody] UpdateVariantSetDto dto)
        {
            try
            {
                await _repo.UpdateAsync(variantSetId, dto.VariantSetName);
                return Ok();
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // =========================
        // TOGGLE STATUS
        // =========================
        [HttpPost("{variantSetId:int}/toggle")]
        public async Task<IActionResult> Toggle(int variantSetId)
        {
            await _repo.ToggleStatusAsync(variantSetId);
            return Ok();
        }
    }
}