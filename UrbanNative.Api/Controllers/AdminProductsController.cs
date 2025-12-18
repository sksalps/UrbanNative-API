using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrbanNative.Application.DTOs;
using UrbanNative.Application.Interfaces;

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

    private int AdminId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // LIST
    [HttpGet]
    public async Task<IActionResult> GetAllProducts(
        string? search,
        string? approvalStatus,
        bool? isActive)
    {
        var products = await _productService.GetAdminProductsAsync(
            search, approvalStatus, isActive);

        return Ok(products);
    }

    // ✅ FIXED ADMIN VIEW
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _productService.GetAdminProductByIdAsync(id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }


    // APPROVE
    [HttpPost("{id:int}/approve")]
    public async Task<IActionResult> Approve(
        int id, [FromBody] ApproveProductRequest req)
    {
        await _productService.ApproveProductAsync(
            id, AdminId, req?.Remark);

        return Ok();
    }

    // REJECT
    [HttpPost("{id:int}/reject")]
    public async Task<IActionResult> Reject(
        int id, [FromBody] RejectProductRequest req)
    {
        await _productService.RejectProductAsync(
            id, AdminId, req.Reason);

        return Ok();
    }

    [HttpPost("{id:int}/toggle-active")]
    public async Task<IActionResult> ToggleActive(int id)
    {
        await _productService.ToggleActiveAsync(id);
        return Ok();
    }
}
