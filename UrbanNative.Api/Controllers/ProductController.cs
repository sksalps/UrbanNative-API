using Microsoft.AspNetCore.Mvc;
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
        // 1️⃣ ADD PRODUCT (Vendor)
        // ===========================================
        [HttpPost("add")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            var id = await _productService.AddProductAsync(product);

            if (id == -1)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Vendor is not approved"
                });
            }

            return Ok(new
            {
                Success = true,
                ProductID = id,
                Message = await _messageService.GetMessageAsync("PRODUCT_ADDED")
            });
        }

        // ===========================================
        // 2️⃣ ADD PRODUCT IMAGE
        // ===========================================
        [HttpPost("add-image")]
        public async Task<IActionResult> AddImage(int productId, string imageUrl, bool isPrimary = false)
        {
            bool result = await _productService.AddProductImageAsync(productId, imageUrl, isPrimary);

            return Ok(new
            {
                Success = result,
                Message = await _messageService.GetMessageAsync("PRODUCT_IMAGE_ADDED")
            });
        }

        // ===========================================
        // 3️⃣ UPDATE PRODUCT
        // ===========================================
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            bool result = await _productService.UpdateProductAsync(product);

            return Ok(new
            {
                Success = result,
                Message = await _messageService.GetMessageAsync("PRODUCT_UPDATED")
            });
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
        [HttpGet("approved")]
        public async Task<IActionResult> GetApprovedProducts()
        {
            var list = await _productService.GetApprovedProductsAsync();
            return Ok(list);
        }

        // ===========================================
        // 6️⃣ LIST PENDING PRODUCTS (Admin)
        // ===========================================
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingProducts()
        {
            var list = await _productService.GetPendingProductsAsync();
            return Ok(list);
        }

        // ===========================================
        // 7️⃣ APPROVE PRODUCT (Admin)
        // ===========================================
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> Approve(int id, int adminId = 1)
        {
            bool result = await _productService.ApproveProductAsync(id, adminId);

            return Ok(new
            {
                Success = result,
                Message = await _messageService.GetMessageAsync("PRODUCT_APPROVED")
            });
        }

        // ===========================================
        // 8️⃣ REJECT PRODUCT (Admin)
        // ===========================================
        [HttpPut("reject/{id}")]
        public async Task<IActionResult> Reject(int id, string reason)
        {
            bool result = await _productService.RejectProductAsync(id, reason);

            return Ok(new
            {
                Success = result,
                Message = await _messageService.GetMessageAsync("PRODUCT_REJECTED")
            });
        }
    }
}