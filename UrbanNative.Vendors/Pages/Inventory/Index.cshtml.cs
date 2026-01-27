using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using UrbanNative.Application.DTOs.Vendors.Inventory;
using UrbanNative.Vendors.Services.Interfaces;
using UrbanNative.Vendors.Services; // ISkuFilterService

namespace UrbanNative.Vendors.Pages.Inventory
{
    public class IndexModel : PageModel
    {
        private readonly IVendorInventoryService _inventoryService;
        private readonly IVendorProductService _warehouseService;
        private readonly ISkuFilterService _skuFilterService;

        public IndexModel(
            IVendorInventoryService inventoryService,
            IVendorProductService warehouseService,
            ISkuFilterService skuFilterService)
        {
            _inventoryService = inventoryService;
            _warehouseService = warehouseService;
            _skuFilterService = skuFilterService;
        }
        //Summary head
        public string? SelectedCategoryName { get; set; }
        public string? SelectedProductName { get; set; }
        public string? SelectedWarehouseName { get; set; }


        // ---------------- FILTER CONTEXT ----------------

        [BindProperty(SupportsGet = true)]
        public int SkuId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? ProductId { get; set; }
        public string? SelectedSkuDisplay { get; set; }
        public bool HasCategory => CategoryId.HasValue;
        public bool HasProduct => ProductId.HasValue;
        public bool HasSku => SkuId > 0;


        // ---------------- WAREHOUSE / DATE ----------------

        [BindProperty(SupportsGet = true)]
        public int? AddressId { get; set; }   // null | 0 | >0

        [BindProperty(SupportsGet = true)]
        public DateTime FromDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime ToDate { get; set; }

        // ---------------- DROPDOWNS ----------------

        public List<SelectListItem> CategoryList { get; set; } = new();
        public List<SelectListItem> ProductList { get; set; } = new();
        public List<SelectListItem> WarehouseList { get; set; } = new();

        // ---------------- RESULTS ----------------

        public VendorInventorySummaryDto Summary { get; set; } = new();
        public IReadOnlyList<VendorInventoryLogDto> Logs { get; set; }
            = new List<VendorInventoryLogDto>();

        public async Task OnGetAsync(int page = 1)
        {
            // ===============================
            // 0️⃣ Capture ORIGINAL values
            // ===============================
            int? originalCategoryId = CategoryId;
            int? originalProductId = ProductId;
            int originalSkuId = SkuId;

            bool isInitialSkuLanding =
                originalSkuId > 0 &&
                originalCategoryId == null &&
                originalProductId == null;

            // ===============================
            // 1️⃣ Date defaults
            // ===============================
            if (FromDate == default)
                FromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            if (ToDate == default)
                ToDate = DateTime.Today;

            // ===============================
            // 2️⃣ Load Categories
            // ===============================
            CategoryList = (await _skuFilterService.GetCategoriesAsync())
                .Select(c => new SelectListItem(c.CategoryName, c.CategoryId.ToString()))
                .ToList();

            // ===============================
            // 3️⃣ Resolve SKU context
            // ===============================
            if (SkuId > 0)
            {
                var skuContext = await _skuFilterService.GetSkuContextAsync(SkuId);
                if (skuContext != null)
                {
                    CategoryId = skuContext.CategoryId;
                    ProductId = skuContext.ProductId;
                    SelectedSkuDisplay = $"{skuContext.SKUCode} | {skuContext.VariantText}";
                }
            }

            // ===============================
            // 4️⃣ RESET RULES (FINAL)
            // ===============================
            if (!isInitialSkuLanding)
            {
                // Category changed by user → reset Product + SKU
                if (originalCategoryId.HasValue
                    && CategoryId.HasValue
                    && originalCategoryId != CategoryId)
                {
                    ProductId = null;
                    SkuId = 0;
                    SelectedSkuDisplay = null;
                }
                // Product changed by user → reset SKU only
                else if (originalProductId.HasValue
                         && ProductId.HasValue
                         && originalProductId != ProductId)
                {
                    SkuId = 0;
                    SelectedSkuDisplay = null;
                }
            }


            // ===============================
            // 5️⃣ Load Products
            // ===============================
            

            // ===============================
            // Display Names for Summary Header
            // ===============================
            if (CategoryId.HasValue)
            {
                ProductList = (await _skuFilterService.GetProductsAsync(CategoryId.Value))
                    .Select(p => new SelectListItem(p.ProductName, p.ProductId.ToString()))
                    .ToList();
                SelectedCategoryName = CategoryList
                    .FirstOrDefault(c => c.Value == CategoryId.Value.ToString())?.Text;
            }

            if (ProductId.HasValue)
            {
                SelectedProductName = ProductList
                    .FirstOrDefault(p => p.Value == ProductId.Value.ToString())?.Text;
            }

            if (AddressId.HasValue)
            {
                SelectedWarehouseName = WarehouseList
                    .FirstOrDefault(w => w.Value == AddressId.Value.ToString())?.Text;
            }


            // ===============================
            // 6️⃣ Warehouses
            // ===============================
            WarehouseList = await _warehouseService.GetVendorWarehousesAsync();
            WarehouseList.Insert(0, new SelectListItem("All Warehouses", "0"));

            if (AddressId == null)
            {
                AddressId = int.Parse(WarehouseList.First(w => w.Value != "0").Value);
            }

            // ===============================
            // 7️⃣ Inventory load
            // ===============================
            if (SkuId <= 0)
                return;

            Summary = await _inventoryService.GetInventorySummaryAsync(
                SkuId, AddressId, FromDate, ToDate);

            Logs = await _inventoryService.GetInventoryLogsAsync(
                SkuId, AddressId, FromDate, ToDate, page, 20);
        }

    }
}
