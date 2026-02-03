using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using UrbanNative.Application.DTOs.CommonCrossDashboard;
using UrbanNative.Application.DTOs.Vendors.Inventory;
using UrbanNative.Domain.Exceptions;
using UrbanNative.Vendors.Services.Interfaces;

public class AddSkuModel : PageModel
{
    private readonly ISkuFilterService _skuFilterService;
    private readonly IVendorAddInventoryService _inventoryService;

    public AddSkuModel(
        ISkuFilterService skuFilterService,
        IVendorAddInventoryService inventoryService)
    {
        _skuFilterService = skuFilterService;
        _inventoryService = inventoryService;
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

    [BindProperty(SupportsGet = true)]
    public int? WarehouseId { get; set; }

    public bool HasSku => SkuId.HasValue && SkuId.Value > 0;

    // ===============================
    // FORM
    // ===============================
    [BindProperty]
    public int Quantity { get; set; }

    [BindProperty]
    public string? Remarks { get; set; }

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

    // ================= GET =================
    public async Task OnGetAsync(int? skuId)
    {
        SkuId = skuId;

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
        if (!ModelState.IsValid || !HasSku || !WarehouseId.HasValue)
        {
            await LoadSelectorsAsync();
            await LoadSkuContextAsync();
            return Page();
        }

        var result = await _inventoryService.AddSkuInventoryAsync(
            SkuId!.Value,
            WarehouseId.Value,
            Quantity,
            Remarks);

        if (!result.Success)
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
                SkuId = null;
                WarehouseId = null;
            }
            else if (originalProductId.HasValue &&
                     ProductId.HasValue &&
                     originalProductId != ProductId)
            {
                SkuId = null;
                WarehouseId = null;
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

        if (!WarehouseId.HasValue)
        {
            WarehouseId = warehouses
                .FirstOrDefault(w => w.IsPrimary)
                ?.VendorWarehouseAddressID;
        }

        if (WarehouseId.HasValue)
        {
            var wh = await _skuFilterService.GetWarehousePreviewAsync(WarehouseId.Value);
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
            Summary = await _inventoryService.GetSkuStockSummaryAsync(SkuId.Value, WarehouseId ?? 0);
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
