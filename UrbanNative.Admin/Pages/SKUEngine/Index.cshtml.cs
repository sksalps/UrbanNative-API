using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using UrbanNative.Application.DTOs.AdminSKU;

namespace UrbanNative.Admin.Pages.SKUEngine
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _http;

        public IndexModel(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public List<AdminSkuOverviewDto> Items { get; set; } = [];

        public async Task OnGetAsync()
        {
            Items = await _http.GetFromJsonAsync<List<AdminSkuOverviewDto>>(
                "api/admin/products/0/skus/overview");

            foreach (var item in Items)
            {
                if (!item.HasVariants)
                {
                    item.Status = "No Variants";
                    item.StatusCss = "secondary";
                }
                else if (item.TotalSkus == 0)
                {
                    item.Status = "No SKU";
                    item.StatusCss = "danger";
                }
                else
                {
                    item.Status = "Active";
                    item.StatusCss = "success";
                }
            }
        }
    }
}
