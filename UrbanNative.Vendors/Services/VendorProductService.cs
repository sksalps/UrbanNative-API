using Microsoft.AspNetCore.Mvc.Rendering;
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

        /* =========================================================
           STEP-1 : PRODUCT LIST
           ========================================================= */

        public async Task<List<VendorProductListDto>> GetMyProductsAsync(
            string? search,
            int? categoryId,
            int? hsnId)
        {
            var url =
                $"/api/vendors/products" +
                $"?q={Uri.EscapeDataString(search ?? string.Empty)}" +
                $"&categoryId={categoryId}" +
                $"&hsnId={hsnId}";

            var res = await _http.GetAsync(url);
            res.EnsureSuccessStatusCode();

            return await res.Content
                .ReadFromJsonAsync<List<VendorProductListDto>>() ?? new();
        }

        public async Task<List<CategoryLookupDto>> GetVendorCategoriesAsync()
        {
            var res = await _http.GetAsync($"/api/vendors/products/categories");
            res.EnsureSuccessStatusCode();

            return await res.Content
                .ReadFromJsonAsync<List<CategoryLookupDto>>() ?? new();
        }

        public async Task<List<HsnLookupDto>> GetVendorHsnListAsync()
        {
            var res = await _http.GetAsync($"/api/vendors/products/hsn");
            res.EnsureSuccessStatusCode();

            return await res.Content
                .ReadFromJsonAsync<List<HsnLookupDto>>() ?? new();
        }

        /* =========================================================
           STEP-2.1 : CREATE PRODUCT
           ========================================================= */

        public async Task CreateProductAsync(VendorProductCreateDto dto)
        {
            var res = await _http.PostAsJsonAsync(
                $"/api/vendors/products",
                dto);

            res.EnsureSuccessStatusCode();
            var id = await res.Content.ReadFromJsonAsync<int>(); // ✅ now valid
        }

        /* =========================================================
           STEP-2.2 : EDIT PRODUCT
           ========================================================= */

        public async Task<VendorProductEditDto> GetProductForEditAsync(int productId)
        {
            var res = await _http.GetAsync(
                $"/api/vendors/products/{productId}");

            res.EnsureSuccessStatusCode();

            return await res.Content
                .ReadFromJsonAsync<VendorProductEditDto>()
                ?? throw new Exception("Product not found");
        }

        public async Task UpdateProductAsync(VendorProductUpdateDto dto)
        {
            var res = await _http.PutAsJsonAsync(
                $"/api/vendors/products/{dto.ProductID}",
                dto);

            res.EnsureSuccessStatusCode();

        }

        /* =========================================================
           SUPPORT : WAREHOUSE LIST
           ========================================================= */
        public async Task<List<SelectListItem>> GetVendorWarehousesAsync()
        {
            var res = await _http.GetAsync($"/api/vendors/warehouses");
            res.EnsureSuccessStatusCode();

            var data = await res.Content
                .ReadFromJsonAsync<List<VendorWarehouseDto>>() ?? new();

            return data
                .Where(w => w.IsActive)
                .Select(w => new SelectListItem
                {
                    Text = w.AddressName,
                    Value = w.VendorWarehouseAddressID.ToString()
                })
                .ToList();
        }

    }

}
