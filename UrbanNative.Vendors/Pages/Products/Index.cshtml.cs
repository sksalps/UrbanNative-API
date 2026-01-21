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

        public List<VendorProductListDto> Products { get; set; } = [];

        public async Task OnGetAsync()
        {
            Products = await _service.GetMyProductsAsync();
        }
    }

}
