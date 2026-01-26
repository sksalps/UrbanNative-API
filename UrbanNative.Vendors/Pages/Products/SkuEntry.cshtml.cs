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

        public async Task<IActionResult> OnPostAsync(int productId)
        {
            for (int i = 0; i < Skus.Count; i++)
            {
                var price = Skus[i].Price ?? 0;
                var dp = Skus[i].DP ?? 0;

                int effectiveStock;
                if (Skus[i].InitStock.HasValue && Skus[i].InitStock > 0)
                {
                    effectiveStock = Skus[i].CurrentStock ?? 0;
                }
                else
                {
                    effectiveStock = Skus[i].Stock;
                }

                // 1️⃣ DP must not exceed Price (always validate)
                if (dp > price)
                {
                    ModelState.AddModelError(
                        $"Skus[{i}].DP",
                        "Discount Price cannot be greater than Price."
                    );
                }

                // 2️⃣ ONLY validate activation rules IF vendor wants it active
                if (Skus[i].IsActive)
                {
                    if (price == 0 || dp == 0)
                    {
                        ModelState.AddModelError(
                            $"Skus[{i}].IsActive",
                            "SKU cannot be activated when Price or Discount Price is zero."
                        );
                    }

                    if (effectiveStock == 0)
                    {
                        ModelState.AddModelError(
                            $"Skus[{i}].IsActive",
                            "SKU cannot be activated when stock is zero."
                        );
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                Header = await _skuService.GetHeaderAsync(productId);

                // 🔒 Rehydrate display-only fields
                var dbSkus = await _skuService.GetGridAsync(productId);

                for (int i = 0; i < Skus.Count; i++)
                {
                    Skus[i].VariantDisplay = dbSkus[i].VariantDisplay;
                    Skus[i].ImageCount = dbSkus[i].ImageCount;
                    Skus[i].InitStock = dbSkus[i].InitStock;
                    Skus[i].CurrentStock = dbSkus[i].CurrentStock;
                    Skus[i].IsStockLocked = dbSkus[i].IsStockLocked;
                }

                return Page();
            }


            await _skuService.SaveAsync(productId, Skus.Select(s => new VendorSkuSaveDto
            {
                SKUId = s.SKUId,
                Price = s.Price,
                DP=s.DP,
                Stock = s.Stock,
                IsActive = s.IsActive
            }).ToList());

            TempData["Success"] = "SKU details saved successfully.";
            return RedirectToPage(new { productId });
        }


    }
}
