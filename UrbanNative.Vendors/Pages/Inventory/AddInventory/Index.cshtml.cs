using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using UrbanNative.Application.DTOs.CommonCrossDashboard;
using UrbanNative.Application.DTOs.Vendors.Inventory;
using UrbanNative.Domain.Exceptions;
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
            // 1️⃣ Load lookups
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

            // =============================
            // 2️⃣ Resolve Input (AUTHORITATIVE)
            // =============================

            if (productId.HasValue)
            {
                Input.ProductId = productId.Value;

                var product = await _skuFilterService.GetProductByIdAsync(productId.Value);
                if (product == null)
                    return Page();

                Input.CategoryId = product.CategoryId;
                Input.WarehouseId = product.VendorWarehouseAddressId;

                var products = await _skuFilterService.GetProductsAsync(Input.CategoryId.Value);
                ProductList = products
                    .Select(p => new SelectListItem(p.ProductName, p.ProductId.ToString()))
                    .ToList();

                IsSkuLoadAttempted = true;
            }
            else if (categoryId.HasValue)
            {
                Input.CategoryId = categoryId.Value;

                var products = await _skuFilterService.GetProductsAsync(categoryId.Value);
                ProductList = products
                    .Select(p => new SelectListItem(p.ProductName, p.ProductId.ToString()))
                    .ToList();

                IsSkuLoadAttempted = false;
            }
            else
            {
                IsSkuLoadAttempted = false;
            }

            // =============================
            // 3️⃣ Resolve Selected* Metadata (ONCE)
            // =============================

            if (Input.CategoryId > 0)
            {
                SelectedCategoryName = CategoryList
                    .FirstOrDefault(c => c.Value == Input.CategoryId.ToString())
                    ?.Text;
            }

            if (Input.ProductId > 0)
            {
                SelectedProductName = ProductList
                    .FirstOrDefault(p => p.Value == Input.ProductId.ToString())
                    ?.Text;
            }

            if (Input.WarehouseId > 0)
            {
                SelectedWarehouse = warehouses
                    .FirstOrDefault(w =>
                        w.VendorWarehouseAddressID == Input.WarehouseId);
            }

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

            try
            {
                var result = await _inventoryService.AddProductInventoryInAsync(Input);
                TempData["Success"] = result.Message;
                return RedirectToPage("../Index");
            }
            catch (DomainValidationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await ReloadLookupsAsync();
                return Page();
            }

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

            var skus = await _inventoryService.GetProductSkusForInventoryAsync(
                    productId.Value,
                    warehouseId.Value);

            return new JsonResult(skus);
        }



    }

}

