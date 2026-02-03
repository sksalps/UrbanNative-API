using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using UrbanNative.Application.DTOs.Vendors.Inventory;
using UrbanNative.Application.DTOs.CommonCrossDashboard;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Pages.Inventory
{
    public class IndexModel : PageModel
    {
        private readonly IVendorInventoryService _inventoryService;
        //private readonly IVendorProductService _warehouseService;
        private readonly ISkuFilterService _skuFilterService;

        public IndexModel(
            IVendorInventoryService inventoryService,
            //IVendorProductService warehouseService,
            ISkuFilterService skuFilterService)
        {
            _inventoryService = inventoryService;
            //_warehouseService = warehouseService;
            _skuFilterService = skuFilterService;
        }

        // ===============================
        // SUMMARY HEADER
        // ===============================
        public string? SelectedCategoryName { get; set; }
        public string? SelectedProductName { get; set; }
        public string? SelectedWarehouseName { get; set; }
        public string? SelectedSkuDisplay { get; set; }

        // ===============================
        // FILTER CONTEXT
        // ===============================
        [BindProperty(SupportsGet = true)]
        public int SkuId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? ProductId { get; set; }
        public string? SelectedWarehouseFullAddress { get; set; }

        public bool HasCategory => CategoryId.HasValue;
        public bool HasProduct => ProductId.HasValue;
        public bool HasSku => SkuId > 0;

        // ===============================
        // WAREHOUSE / DATE
        // ===============================
        [BindProperty(SupportsGet = true)]
        public int? AddressId { get; set; } // null | 0 | >0

        [BindProperty(SupportsGet = true)]
        public DateTime FromDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime ToDate { get; set; }

        // ===============================
        // DROPDOWNS
        // ===============================
        public List<SelectListItem> CategoryList { get; set; } = new();
        public List<SelectListItem> ProductList { get; set; } = new();
        public List<SelectListItem> WarehouseList { get; set; } = new();

        // ===============================
        // RESULTS
        // ===============================
        public VendorInventorySummaryDto Summary { get; set; } = new();
        public IReadOnlyList<VendorInventoryLogDto> Logs { get; set; }
            = new List<VendorInventoryLogDto>();

        // ===============================
        // MAIN HANDLER
        // ===============================
        public async Task OnGetAsync(int page = 1)
        {
            // ===============================
            // 0️⃣ Capture original values
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
            var categories = await _skuFilterService.GetCategoriesAsync();
            CategoryList = categories
                .Select(c => new SelectListItem(c.CategoryName, c.CategoryId.ToString()))
                .ToList();

            // ===============================
            // 3️⃣ Resolve SKU Context (deep link)
            // ===============================
            if (SkuId > 0)
            {
                var skuContext = await _skuFilterService.GetSkuContextAsync(SkuId);
                if (skuContext != null)
                {
                    CategoryId = skuContext.CategoryId;
                    ProductId = skuContext.ProductId;
                    SelectedSkuDisplay =
                        $"{skuContext.SKUCode} | {skuContext.VariantText}";
                }
            }

            // ===============================
            // 4️⃣ RESET RULES (FINAL & LOCKED)
            // ===============================
            if (!isInitialSkuLanding)
            {
                // Category changed → reset Product + SKU + Warehouse
                if (originalCategoryId.HasValue &&
                    CategoryId.HasValue &&
                    originalCategoryId != CategoryId)
                {
                    ProductId = null;
                    SkuId = 0;
                    SelectedSkuDisplay = null;
                    AddressId = null;
                }
                // Product changed → reset SKU + Warehouse
                else if (originalProductId.HasValue &&
                         ProductId.HasValue &&
                         originalProductId != ProductId)
                {
                    SkuId = 0;
                    SelectedSkuDisplay = null;
                    AddressId = null;
                }
            }

            // ===============================
            // 5️⃣ Load Products (Category scoped)
            // ===============================
            if (CategoryId.HasValue)
            {
                var products = await _skuFilterService.GetProductsAsync(CategoryId.Value);
                ProductList = products
                    .Select(p => new SelectListItem(p.ProductName, p.ProductId.ToString()))
                    .ToList();

                SelectedCategoryName = categories
                    .FirstOrDefault(c => c.CategoryId == CategoryId)?.CategoryName;
            }

            if (ProductId.HasValue)
            {
                SelectedProductName = ProductList
                    .FirstOrDefault(p => p.Value == ProductId.Value.ToString())?.Text;
            }

            // ===============================
            // 6️⃣ Warehouses (AUTHORITATIVE)
            // ===============================
            var warehouses = await _skuFilterService.GetAllWarehousesAsync();

            WarehouseList = warehouses
                .Select(w => new SelectListItem
                {
                    Value = w.VendorWarehouseAddressID.ToString(),
                    Text = w.IsPrimary
                        ? $"{w.AddressName} (Primary)"
                        : w.AddressName
                })
                .ToList();

            WarehouseList.Insert(0, new SelectListItem("All Warehouses", "0"));

            // Resolve default warehouse
            if (!AddressId.HasValue)
            {
                var primary = warehouses.FirstOrDefault(w => w.IsPrimary);
                AddressId = primary?.VendorWarehouseAddressID ?? 0;
            }

            // Resolve warehouse display + full address
            if (AddressId == 0)
            {
                SelectedWarehouseName = "All Warehouses";
                SelectedWarehouseFullAddress = "All vendor warehouses";
            }
            else if (AddressId.HasValue)
            {
                var wh = await _skuFilterService.GetWarehousePreviewAsync(AddressId.Value);

                if (wh != null)
                {
                    SelectedWarehouseName = wh.IsPrimary
                        ? $"{wh.AddressName} (Primary)"
                        : wh.AddressName;

                    SelectedWarehouseFullAddress =
                        (
                            string.Join("\n", new[]
                            {
                                wh.AddressLine1,
                                wh.AddressLine2,
                                wh.Landmark,
                                string.Join(", ",
                                new[]
                                {
                                    wh.CityName,
                                    wh.StateName,
                                    wh.CountryName
                                }.Where(x => !string.IsNullOrWhiteSpace(x))
                            )
                        }.Where(x => !string.IsNullOrWhiteSpace(x)))
                        + (string.IsNullOrWhiteSpace(wh.Pincode)
                            ? ""
                            : $" – {wh.Pincode}")
                    ).Trim();
                }
            }



            // ===============================
            // 7️⃣ Inventory load (SKU required)
            // ===============================
            if (SkuId <= 0)
                return;

            Summary = await _inventoryService.GetInventorySummaryAsync(
                SkuId,
                AddressId,
                FromDate,
                ToDate);

            Logs = await _inventoryService.GetInventoryLogsAsync(
                SkuId,
                AddressId,
                FromDate,
                ToDate,
                page,
                20);
        }
    }
}
