using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrbanNative.Application.Interfaces;

namespace UrbanNative.Api.Controllers
{
    [ApiController]
    [Route("api/admin/products")]
    [Authorize(Roles = "Admin")]
    public class AdminProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public AdminProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // -----------------------------
        // Helper: AdminId from claims
        // -----------------------------
        private int AdminId =>int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        // ======================================================
        // 1️⃣ LIST ALL PRODUCTS (Admin + Filters)
        // ======================================================
        // GET: /api/admin/products?search=&approvalStatus=&isActive=
        [HttpGet]
        public async Task<IActionResult> GetAllProducts(
            [FromQuery] string? search,
            [FromQuery] string? approvalStatus,
            [FromQuery] bool? isActive)
        {
            var products = await _productService.GetAdminProductsAsync(
                search,
                approvalStatus,
                isActive
            );

            return Ok(products);
        }


        // ======================================================
        // 2️⃣ GET PRODUCT DETAILS
        // ======================================================
        // GET: /api/admin/products/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // ======================================================
        // 3️⃣ APPROVE PRODUCT
        // ======================================================
        // POST: /api/admin/products/{id}/approve
        [HttpPost("{id:int}/approve")]
        public async Task<IActionResult> ApproveProduct(int id)
        {
            var result = await _productService.ApproveProductAsync(id, AdminId);

            return Ok(new
            {
                Success = result,
                Message = "Product approved successfully"
            });
        }

        // ======================================================
        // 4️⃣ REJECT PRODUCT
        // ======================================================
        // POST: /api/admin/products/{id}/reject
        [HttpPost("{id:int}/reject")]
        public async Task<IActionResult> RejectProduct(int id, [FromBody] RejectProductRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Reason))
                return BadRequest("Reject reason is required.");

            var result = await _productService.RejectProductAsync(id, req.Reason);

            return Ok(new
            {
                Success = result,
                Message = "Product rejected successfully"
            });
        }

        // ======================================================
        // 5️⃣ ACTIVATE / DEACTIVATE PRODUCT
        // ======================================================
        // POST: /api/admin/products/{id}/toggle-active
        [HttpPost("{id:int}/toggle-active")]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var result = await _productService.ToggleActiveAsync(id);

            return Ok(new
            {
                Success = result,
                Message = "Product status updated"
            });
        }
    }

    // ===============================
    // DTO: Reject product
    // ===============================
    public class RejectProductRequest
    {
        public string Reason { get; set; } = string.Empty;
    }
}
