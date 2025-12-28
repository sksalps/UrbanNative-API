using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using UrbanNative.Application.DTOs.AdminSKU;

namespace UrbanNative.Admin.Pages.SKUEngine
{
    public class ProductSkusModel : PageModel
    {
        private readonly HttpClient _http;

        public ProductSkusModel(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        [BindProperty(SupportsGet = true)]
        public int ProductId { get; set; }

        public List<ProductSkuDetailDto> Skus { get; set; } = [];

        public async Task OnGetAsync()
        {
            Skus = await _http.GetFromJsonAsync<List<ProductSkuDetailDto>>(
                $"api/admin/products/{ProductId}/skus");
        }
    }
}
