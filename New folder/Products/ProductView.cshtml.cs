using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Admin.Services;
using UrbanNative.Application.DTOs;

namespace UrbanNative.Admin.Pages.Products
{
    public class ProductViewModel : PageModel
    {
        private readonly IAdminProductService _productService;

        public ProductViewModel(IAdminProductService productService)
        {
            _productService = productService;
        }

        public AdminProductDto Product { get; set; } = new();
        public List<string> ApprovalHistory { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            Product = product;

            if (!string.IsNullOrWhiteSpace(product.RejectedReason))
            {
                ApprovalHistory = product.RejectedReason?
     .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
     .Select(x => x.Trim())
     .Reverse()
     .ToList()
     ?? new List<string>();

            }



            return Page();
        }
    }
}
