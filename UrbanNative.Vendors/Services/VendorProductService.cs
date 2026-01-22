using System.Net.Http.Json;

using UrbanNative.Application.DTOs.Vendors.Products;
using UrbanNative.Vendors.Services.Interfaces;


        
namespace UrbanNative.Vendors.Services
{
    public class VendorProductService : IVendorProductService
    {
        private readonly HttpClient _http;
        public VendorProductService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        public async Task<List<VendorProductListDto>> GetMyProductsAsync(
            string? search,
            int? categoryId,
            int? hsnId)
        {
            var url =
                $"/api/vendors/products?" +
                $"q={Uri.EscapeDataString(search ?? string.Empty)}" +
                $"&categoryId={categoryId}" +
                $"&hsnId={hsnId}";

            var response = await _http.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadFromJsonAsync<List<VendorProductListDto>>() ?? new();
        }

        public async Task<List<CategoryLookupDto>> GetVendorCategoriesAsync()
        {
            var response = await _http.GetAsync("/api/vendors/products/categories");
            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadFromJsonAsync<List<CategoryLookupDto>>() ?? new();
        }

        public async Task<List<HsnLookupDto>> GetVendorHsnListAsync()
        {
            var response = await _http.GetAsync("/api/vendors/products/hsn");
            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadFromJsonAsync<List<HsnLookupDto>>() ?? new();
        }
    }

}
