using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using UrbanNative.Application.DTOs.Vendors.Inventory;
using UrbanNative.Domain.Exceptions;
using UrbanNative.Vendors.Services.Interfaces;

public class AdjustSkuStockModel : PageModel
{
    private readonly ISkuFilterService _skuFilterService;
    private readonly IVendorAddInventoryService _inventoryService;
    private readonly IVendorInventoryService _inventoryLogService;

    public AdjustSkuStockModel(
        ISkuFilterService skuFilterService,
        IVendorAddInventoryService inventoryService,
        IVendorInventoryService inventoryLogService)
    {
        _skuFilterService = skuFilterService;
        _inventoryService = inventoryService;
        _inventoryLogService = inventoryLogService;
    }
    public string? GlobalErrorMessage { get; set; }
    // ===============================
    // HEADER SUMMARY
    // ===============================
    public string? SelectedCategoryName { get; set; }
    public string? SelectedProductName { get; set; }
    public string? SelectedWarehouseName { get; set; }
    public string? SelectedWarehouseFullAddress { get; set; }
    public string? SelectedSkuDisplay { get; set; }

    // ===============================
    // FILTER CONTEXT (GET)
    // ===============================
    [BindProperty(SupportsGet = true)]
    public int? CategoryId { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? ProductId { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? SkuId { get; set; }

    //[BindProperty(SupportsGet = true)]
    //public int? WarehouseId { get; set; }

    public bool HasSku => AdjustRequest.SKUId.HasValue && AdjustRequest.SKUId.Value > 0;
    //public bool HasWarehouseId => AdjustRequest.WarehouseId>0;

    // ===============================
    // FORM
    // ===============================
    [BindProperty]
    public AdjustInventoryRequestDto AdjustRequest { get; set; } = new();
    //[BindProperty]
    //public string? ChangeType { get; set; }
    //public int Quantity { get; set; }

    //[BindProperty]
    //public string? Remarks { get; set; }

    public bool IsSuccess { get; set; }

    // ===============================
    // DROPDOWNS
    // ===============================
    public List<SelectListItem> CategoryList { get; set; } = new();
    public List<SelectListItem> ProductList { get; set; } = new();
    public List<SelectListItem> WarehouseList { get; set; } = new();

    // ===============================
    // SUMMARY
    // ===============================
    public SkuInventoryStockSummaryDto Summary { get; set; } = new();
    public IReadOnlyList<VendorInventoryLogDto> Logs { get; set; }
            = new List<VendorInventoryLogDto>();
    // ===============================
    // ADJUST REQUEST
    // ===============================
    
    [BindProperty(SupportsGet = true)]
    public DateTime FromDate { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime ToDate { get; set; }

    // ================= GET =================
    public async Task OnGetAsync(int? skuId)
    {
        SkuId = skuId;
        AdjustRequest.SKUId = skuId;

        // 1️⃣ Resolve SKU → Category/Product FIRST (deep link)
        if (SkuId.HasValue && SkuId > 0)
        {
            await ResolveSkuContextAsync();
        }

        // 2️⃣ Load selectors AFTER context is known
        await LoadSelectorsAsync();

        // 3️⃣ Load summary (already correct)
        await LoadSkuContextAsync();


        //await LoadSelectorsAsync();
        //await LoadSkuContextAsync();
    }

    // ================= POST =================
    public async Task<IActionResult> OnPostAsync()
    {
        
        if (!ModelState.IsValid || !HasSku || !AdjustRequest.WarehouseId.HasValue)
        {
            ModelState.AddModelError(string.Empty,"Please select SKU and Warehouse.");
            await LoadSelectorsAsync();
            await LoadSkuContextAsync();
            return Page();
        }

        //var result = await _inventoryService.AddSkuInventoryAsync(           SkuId!.Value,            WarehouseId.Value, Quantity,      Remarks);
        var result =     await _inventoryService.AdjustInventoryAsync(AdjustRequest);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            await LoadSelectorsAsync();
            await LoadSkuContextAsync();
            return Page();

        }

        IsSuccess = true;

        await LoadSelectorsAsync();
        await LoadSkuContextAsync();

        return Page();
    }

    // ===============================
    // LOAD SELECTORS (PART 3.2 STYLE)
    // ===============================
    private async Task LoadSelectorsAsync()
    {
        int? originalCategoryId = CategoryId;
        int? originalProductId = ProductId;
        int? originalSkuId = SkuId;

        // 🔒 IMPORTANT: detect initial deep-link by SKU
        bool isInitialSkuLanding =
            originalSkuId.HasValue &&
            originalSkuId > 0 &&
            !originalCategoryId.HasValue &&
            !originalProductId.HasValue;
        if (FromDate == default)
            FromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

        if (ToDate == default)
            ToDate = DateTime.Today;
        // ===============================
        // 1️⃣ Categories
        // ===============================
        var categories = await _skuFilterService.GetCategoriesForFilter();
        CategoryList = categories
            .Select(c => new SelectListItem(c.CategoryName, c.CategoryId.ToString()))
            .ToList();


        // ===============================
        // 2️⃣ RESET LOGIC (SKIP FOR SKU DEEP LINK)
        // ===============================
        if (!isInitialSkuLanding)
        {
            if (originalCategoryId.HasValue &&
                CategoryId.HasValue &&
                originalCategoryId != CategoryId)
            {
                ProductId = null;
                AdjustRequest.SKUId = null;
                AdjustRequest.WarehouseId = null;
            }
            else if (originalProductId.HasValue &&
                     ProductId.HasValue &&
                     originalProductId != ProductId)
            {
                AdjustRequest.SKUId = null;
                AdjustRequest.WarehouseId = null;
            }
        }

        // ===============================
        // 3️⃣ Products (Category scoped)
        // ===============================
        if (CategoryId.HasValue)
        {
            var products = await _skuFilterService.GetProductsAsync(CategoryId.Value);
            ProductList = products
                .Select(p => new SelectListItem(p.ProductName, p.ProductId.ToString()))
                .ToList();

            SelectedCategoryName =
                categories.FirstOrDefault(x => x.CategoryId == CategoryId)?.CategoryName;
        }

        if (ProductId.HasValue)
        {
            SelectedProductName =
                ProductList.FirstOrDefault(p => p.Value == ProductId.Value.ToString())?.Text;
        }

        // ===============================
        // 4️⃣ Warehouses (ACTIVE ONLY – ADD FLOW)
        // ===============================
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

        if (!AdjustRequest.WarehouseId.HasValue)
        {
            AdjustRequest.WarehouseId = warehouses
                .FirstOrDefault(w => w.IsPrimary)
                ?.VendorWarehouseAddressID;
        }

        if (AdjustRequest.WarehouseId.HasValue)
        {
            var wh = await _skuFilterService.GetWarehousePreviewAsync(AdjustRequest.WarehouseId.Value);
            if (wh != null)
            {
                SelectedWarehouseName = wh.IsPrimary
                    ? $"{wh.AddressName} (Primary)"
                    : wh.AddressName;

                SelectedWarehouseFullAddress = string.Join(", ",
                    new[]
                    {
                    wh.AddressLine1,
                    wh.AddressLine2,
                    wh.Landmark,
                    wh.CityName,
                    wh.StateName,
                    wh.CountryName,
                    wh.Pincode
                    }.Where(x => !string.IsNullOrWhiteSpace(x)));
            }
        }
    }

    private async Task ResolveSkuContextAsync()
    {
        var skuContext = await _skuFilterService.GetSkuContextAsync(SkuId.Value);
        if (skuContext == null)
            return;

        CategoryId = skuContext.CategoryId;
        ProductId = skuContext.ProductId;
        SelectedSkuDisplay =
            $"{skuContext.SKUCode} | {skuContext.VariantText}";
    }


    // ===============================
    // LOAD SKU CONTEXT + SUMMARY
    // ===============================
    private async Task LoadSkuContextAsync()
    {
        if (!HasSku)
            return;

        var skuContext = await _skuFilterService.GetSkuContextAsync(SkuId!.Value);
        if (skuContext == null)
            return;

        CategoryId = skuContext.CategoryId;
        ProductId = skuContext.ProductId;

        SelectedSkuDisplay =
            $"{skuContext.SKUCode} | {skuContext.VariantText}";



        try
        {
            Summary = await _inventoryService.GetSkuStockSummaryAsync(SkuId.Value, AdjustRequest.WarehouseId ?? 0);
            Logs = await _inventoryLogService.GetInventoryLogsAsync(
                        SkuId.Value,
                        AdjustRequest.WarehouseId.Value,
                        FromDate,
                        ToDate,
                        1,
                        20);
        }
        catch (DomainValidationException ex)
        {
            // 👇 THIS IS THE MISSING PIECE
            GlobalErrorMessage = ex.Message;

            // keep page alive
            Summary = null;
        }
    }
}
