using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using UrbanNative.Application.DTOs.CommonCrossDashboard;
using UrbanNative.Application.DTOs.Vendors.Inventory;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Pages.Inventory.AddInventory
{
    public class IndexModel : PageModel
    {
        private readonly IVendorAddInventoryService _inventoryService;
        private readonly ISkuFilterService _skuFilterService;

        public IndexModel(
            IVendorAddInventoryService inventoryService,
            ISkuFilterService skuFilterService)
        {
            _inventoryService = inventoryService;
            _skuFilterService = skuFilterService;
        }

        // =============================
        // Lookups (DDL only)
        // =============================
        public List<SelectListItem> CategoryList { get; set; } = new();
        public List<SelectListItem> ProductList { get; set; } = new();
        public List<SelectListItem> WarehouseList { get; set; } = new();

        public string SelectedCategoryName { get; set; }
        public string SelectedProductName { get; set; }
        public CommonWarehouseDto SelectedWarehouse { get; set; } = new();


        // =============================
        // Input
        // =============================

        [BindProperty]
        public ProductInventoryInRequestDto Input { get; set; } = new();
        public bool IsProductSelected => Input.ProductId > 0;
        public bool IsSkuLoadAttempted { get; set; }

        // =============================
        // GET
        // =============================

        public async Task<IActionResult> OnGetAsync(
            int? categoryId,
            int? productId)
        {
            // 1️⃣ Categories (all where vendor has products)
            var categories = await _skuFilterService.GetCategoriesForFilter();
            CategoryList = categories
                .Select(c => new SelectListItem(c.CategoryName, c.CategoryId.ToString()))
                .ToList();

            // 2️⃣ Warehouses (ACTIVE vendor warehouses only)
            var warehouses = await _skuFilterService.GetVendorActiveWarehouseAsync();
            WarehouseList = warehouses
                .Select(w => new SelectListItem
                {
                    Value = w.VendorWarehouseAddressID.ToString(),
                    Text = w.IsPrimary
                        ? $"{w.AddressName} (Primary)"
                        : w.AddressName
                })
                .ToList();


            // =============================
            // AUTO-LOAD LOGIC (LOCKED)
            // =============================

            // CASE 1: ProductId is supplied (deep-link / redirect)
            if (productId.HasValue)
            {
                Input.ProductId = productId.Value;

                var product = await _skuFilterService.GetProductByIdAsync(productId.Value);
                if (product == null)
                {
                    // Product not accessible → fallback
                    return Page();
                }

                Input.CategoryId = product.CategoryId;
                Input.WarehouseId = product.VendorWarehouseAddressId;

                var products = await _skuFilterService.GetProductsAsync(Input.CategoryId.Value);
                ProductList = products
                    .Select(p => new SelectListItem(p.ProductName, p.ProductId.ToString()))
                    .ToList();

                return Page();
            }




            // CASE 2: Only CategoryId supplied
            // CASE 2: Category only
            if (categoryId.HasValue)
            {
                Input.CategoryId = categoryId.Value;
                IsSkuLoadAttempted = false;

                var products = await _skuFilterService.GetProductsAsync(categoryId.Value);
                ProductList = products
                    .Select(p => new SelectListItem(p.ProductName, p.ProductId.ToString()))
                    .ToList();
            }


            // CASE 3: Fresh page
            IsSkuLoadAttempted = false;
            return Page();
        }

        // =============================
        // POST
        // =============================

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Input.Items");

            if (!ModelState.IsValid)
            {
                await ReloadLookupsAsync();
                return Page();
            }


            var result = await _inventoryService.AddProductInventoryInAsync(Input);

            TempData["Success"] = result.Message;
            return RedirectToPage("../Index");
        }

        // =============================
        // Helpers
        // =============================

        private async Task ReloadLookupsAsync()
        {
            var categories = await _skuFilterService.GetCategoriesForFilter();
            CategoryList = categories
                .Select(c => new SelectListItem(c.CategoryName, c.CategoryId.ToString()))
                .ToList();

            var warehouses = await _skuFilterService.GetVendorActiveWarehouseAsync();
            WarehouseList = warehouses
                .Select(w => new SelectListItem
                {
                    Value = w.VendorWarehouseAddressID.ToString(),
                    Text = w.IsPrimary
                        ? $"{w.AddressName} (Primary)"
                        : w.AddressName
                })
                .ToList();

            if (Input.CategoryId > 0)
            {
                var products = await _skuFilterService.GetProductsAsync(Input.CategoryId.Value);
                ProductList = products
                    .Select(p => new SelectListItem(p.ProductName, p.ProductId.ToString()))
                    .ToList();
            }
        }

        public async Task<IActionResult> OnGetSkusAsync(
        int? productId,
        int? warehouseId)
        {
            if (!productId.HasValue || !warehouseId.HasValue)
                return new JsonResult(Array.Empty<ProductAddInventorySkuGridDto>());

            var skus = await _inventoryService
                .GetProductSkusForInventoryAsync(
                    productId.Value,
                    warehouseId.Value);

            return new JsonResult(skus);
        }



    }

}

