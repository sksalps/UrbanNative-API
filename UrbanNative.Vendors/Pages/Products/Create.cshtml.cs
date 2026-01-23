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

        public List<SelectListItem> CategoryList { get; set; } = [];
        public List<SelectListItem> WarehouseList { get; set; } = [];

        public string HsnPreview { get; set; } = "Auto from category";

        public async Task OnGetAsync()
        {
            CategoryList = (await _service.GetVendorActiveCategoriesAsync())
                .Select(c => new SelectListItem(c.CategoryName, c.CategoryID.ToString()))
                .ToList();

            WarehouseList = await _service.GetVendorWarehousesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _service.CreateProductAsync(Product);
            return RedirectToPage("./Index");
        }
    }
}
