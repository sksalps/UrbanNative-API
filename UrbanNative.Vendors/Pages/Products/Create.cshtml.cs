using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using UrbanNative.Application.DTOs.Vendors.Products;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly IVendorProductService _service;

        public CreateModel(IVendorProductService service)
        {
            _service = service;
        }

        [BindProperty]
        public VendorProductCreateDto Product { get; set; } = new();

        public List<SelectListItem> CategoryList { get; set; } = new();
        public List<SelectListItem> WarehouseList { get; set; } = new();
        public List<SelectListItem> ReturnPolicyList { get; set; } = new();

        public async Task OnGetAsync()
        {
            CategoryList = (await _service.GetVendorActiveCategoriesAsync())
                .Select(c => new SelectListItem
                {
                    Text = c.CategoryName,
                    Value = c.CategoryID.ToString()
                }).ToList();

            WarehouseList = await _service.GetVendorWarehousesAsync();
            ReturnPolicyList = await _service.GetVendorReturnPolicyAsync();
        }

        // 🔹 AJAX: Category → HSN/GST
        public async Task<JsonResult> OnGetCategoryHsnAsync(int categoryId)
        {
            var data = await _service.GetCategoryHsnPreviewAsync(categoryId);
            return new JsonResult(data);
        }

        // 🔹 AJAX: Warehouse → Address preview
        public async Task<JsonResult> OnGetWarehousePreviewAsync(int warehouseId)
        {
            var data = await _service.GetWarehousePreviewAsync(warehouseId);
            return new JsonResult(data);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(); // rebind dropdowns
                return Page();
            }

            await _service.CreateProductAsync(Product);

            TempData["Success"] = "Product created successfully";
            return RedirectToPage("./Index");
        }
    }
}
