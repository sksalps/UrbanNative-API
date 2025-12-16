using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrbanNative.Application.Interfaces;
using UrbanNative.Api.Services;
using UrbanNative.Domain.Entities;

namespace UrbanNative.Api.Controllers
{
    [ApiController]

    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMessageService _messageService;

        public ProductController(IProductService productService, IMessageService messageService)
        {
            _productService = productService;
            _messageService = messageService;
        }

        // ===========================================
        // 4️⃣ GET PRODUCT DETAILS
        // ===========================================
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProduct(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);

            if (product == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = await _messageService.GetMessageAsync("PRODUCT_NOT_FOUND")
                });
            }

            return Ok(new { Success = true, Product = product });
        }

        // ===========================================
        // 5️⃣ LIST APPROVED PRODUCTS
        // ===========================================

    }
}