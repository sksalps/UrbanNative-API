using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using UrbanNative.Application.DTOs.AdminSKU;

namespace UrbanNative.Admin.Pages.SKUEngine.VendorCoverage
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _http;

        public IndexModel(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        // =========================
        // Query Params
        // =========================
        [BindProperty(SupportsGet = true)]
        public int ProductId { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IncludeInactiveVendors { get; set; } = false;

        // =========================
        // View Models
        // =========================
        public ProductSkuCoverageHeaderDto ProductHeader { get; set; }
            = new();

        public List<AdminVendorSkuCoverageDto> Vendors { get; set; }
            = new();

        // =========================
        // GET
        // =========================
        public async Task OnGetAsync()
        {
            if (ProductId <= 0)
                return;

            // 1️⃣ Load product header
            ProductHeader =
                await _http.GetFromJsonAsync<ProductSkuCoverageHeaderDto>(
                    $"api/admin/products/{ProductId}/skus/coverage-header"
                );

            // 2️⃣ Load vendor coverage
            Vendors =
                await _http.GetFromJsonAsync<List<AdminVendorSkuCoverageDto>>(
                    $"api/admin/products/{ProductId}/skus/vendor-coverage" +
                    $"?includeInactiveVendors={IncludeInactiveVendors}"
                );

            // 3️⃣ Compute UI status
            foreach (var v in Vendors)
            {
                if (v.TotalCombinations == 0)
                {
                    v.Status = "No Variants";
                    v.StatusCss = "secondary";
                }
                else if (v.CoveragePercent == 100)
                {
                    v.Status = "Complete";
                    v.StatusCss = "success";
                }
                else if (v.CoveragePercent > 0)
                {
                    v.Status = "Partial";
                    v.StatusCss = "warning";
                }
                else
                {
                    v.Status = "Missing";
                    v.StatusCss = "danger";
                }
            }
        }
    }
}
