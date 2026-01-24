using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using UrbanNative.Application.DTOs.Vendors.Products;
using UrbanNative.Vendors.Services.Interfaces;

public class EditModel : PageModel
{
    private readonly IVendorProductService _service;

    public EditModel(IVendorProductService service)
    {
        _service = service;
    }

    // UI reference
    public VendorProductEditDto ViewProduct { get; set; } = new();

    // POST model
    [BindProperty]
    public VendorProductUpdateDto Product { get; set; } = new();

    public List<SelectListItem> CategoryList { get; set; } = [];
    public List<SelectListItem> WarehouseList { get; set; } = [];
    public List<SelectListItem> ReturnPolicyList { get; set; } = [];

    public bool IsApproved => ViewProduct.ApprovalStatus == "APPROVED";

    public async Task OnGetAsync(int id)
    {
        ViewProduct = await _service.GetProductForEditAsync(id);

        Product = new VendorProductUpdateDto
        {
            ProductID = ViewProduct.ProductID,
            ProductName = ViewProduct.ProductName,
            Description = ViewProduct.Description,
            MRP = ViewProduct.MRP,
            DiscountPrice = ViewProduct.DiscountPrice,
            CategoryID = ViewProduct.CategoryID,
            HasVariants = ViewProduct.HasVariants,
            VendorSharedMargin = ViewProduct.VendorSharedMargin,
            VendorWarehouseAddressID = ViewProduct.VendorWarehouseAddressID,
            ReturnPolicyID = ViewProduct.ReturnPolicyID
        };

        var categories = IsApproved
    ? await _service.GetVendorAllCategoriesAsync()
    : await _service.GetVendorActiveCategoriesAsync();

        CategoryList = categories
            .Select(c => new SelectListItem
            {
                Text = c.CategoryName,
                Value = c.CategoryID.ToString()
            })
            .ToList();


        WarehouseList = await _service.GetVendorWarehousesAsync();
        ReturnPolicyList = await _service.GetVendorReturnPolicyAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await OnGetAsync(Product.ProductID);
            return Page();
        }

        await _service.UpdateProductAsync(Product);

        TempData["SuccessMessage"] = "Product updated successfully.";
        return RedirectToPage("Edit", new { id = Product.ProductID });
    }


    // ================================
    // AJAX: Category → HSN / GST / Margin
    // ================================
    public async Task<IActionResult> OnGetCategoryHsnAsync(int categoryId)
    {
        var dto = await _service.GetCategoryHsnPreviewAsync(categoryId);
        return new JsonResult(dto);
    }

    // ================================
    // AJAX: Warehouse → Address Preview
    // ================================
    public async Task<IActionResult> OnGetWarehousePreviewAsync(int warehouseId)
    {
        var dto = await _service.GetWarehousePreviewAsync(warehouseId);
        return new JsonResult(dto);
    }

}
