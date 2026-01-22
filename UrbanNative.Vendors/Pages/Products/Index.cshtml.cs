using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrbanNative.Application.DTOs.Vendors.Products;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IVendorProductService _service;

        public IndexModel(IVendorProductService service)
        {
            _service = service;
        }

        // Filters
        [BindProperty(SupportsGet = true)]
        public string? SearchText { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? HsnId { get; set; }

        // Data
        public List<VendorProductListDto> Products { get; set; } = [];
        public List<CategoryLookupDto> CategoryList { get; set; } = [];
        public List<HsnLookupDto> HsnList { get; set; } = [];

        public async Task OnGetAsync()
        {
            CategoryList = await _service.GetVendorCategoriesAsync();
            HsnList = await _service.GetVendorHsnListAsync();

            Products = await _service.GetMyProductsAsync(
                SearchText,
                CategoryId,
                HsnId
            );
        }
    }

}
