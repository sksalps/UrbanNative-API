using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.Vendors;
using UrbanNative.Application.DTOs.Vendors.Products;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Pages.Products
{
    public class SkuEntryModel : PageModel
    {
        private readonly IVendorSkuService _skuService;

        public SkuEntryModel(IVendorSkuService skuService)
        {
            _skuService = skuService;
        }

        // ===============================
        // READ-ONLY HEADER
        // ===============================
        public VendorSkuHeaderDto Header { get; set; } = new();

        // ===============================
        // SKU GRID (Editable)
        // ===============================
        [BindProperty]
        public List<VendorSkuGridDto> Skus { get; set; } = new();

        // ===============================
        // GET
        // ===============================
        public async Task<IActionResult> OnGetAsync(int productId)
        {
            Header = await _skuService.GetHeaderAsync(productId);
            Skus = await _skuService.GetGridAsync(productId);

            return Page();
        }

        // ===============================
        // POST
        // ===============================
        public async Task<IActionResult> OnPostAsync(int productId)
        {
            var saveDtos = Skus.Select(s => new VendorSkuSaveDto
            {
                SKUId = s.SKUId,
                Price = s.Price,
                Stock = s.Stock,
                IsActive = s.IsActive
            }).ToList();

            await _skuService.SaveAsync(productId, saveDtos);

            TempData["Success"] = "SKU details saved successfully.";
            return RedirectToPage(new { productId });
        }
    }
}
