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

    // 🔵 Used for UI rendering & control
    public VendorProductEditDto ViewProduct { get; set; } = new();

    // 🟡 Used for form POST
    [BindProperty]
    public VendorProductUpdateDto Product { get; set; } = new();

    public List<SelectListItem> CategoryList { get; set; } = [];
    public List<SelectListItem> WarehouseList { get; set; } = [];
    public List<SelectListItem> ReturnPolicyList { get; set; } = [];
    public bool IsApproved => ViewProduct.ApprovalStatus == "APPROVED";

    public async Task OnGetAsync(int id)
    {
        ViewProduct = await _service.GetProductForEditAsync(id);

        // map editable fields into update DTO
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

        CategoryList = (await _service.GetVendorAllCategoriesAsync())
            .Select(c => new SelectListItem(c.CategoryName, c.CategoryID.ToString()))
            .ToList();

        WarehouseList = await _service.GetVendorWarehousesAsync();
        ReturnPolicyList = await _service.GetVendorReturnPolicyAsync();

    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        await _service.UpdateProductAsync(Product);
        return RedirectToPage("./Index");
    }
}
