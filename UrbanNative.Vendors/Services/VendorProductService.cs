using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Json;
using UrbanNative.Application.DTOs.Vendors.Products;
using UrbanNative.Application.DTOs.CommonCrossDashboard;
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
        //Active Cat use in product add from vendor side
        public async Task<List<CategoryLookupDto>> GetVendorActiveCategoriesAsync()
        {
            var res = await _http.GetAsync($"/api/skufilter/categories/create");
            res.EnsureSuccessStatusCode();

            return await res.Content
                .ReadFromJsonAsync<List<CategoryLookupDto>>() ?? new();
        }
        //Active Inactive use with product list & filter with vendor
        public async Task<List<CategoryLookupDto>> GetVendorAllCategoriesAsync() 
        {
            var res = await _http.GetAsync($"/api/skufilter/categories/vendor");
            res.EnsureSuccessStatusCode();

            return await res.Content
                .ReadFromJsonAsync<List<CategoryLookupDto>>() ?? new();
        }
        /*public async Task<List<CategoryLookupDto>> GetCategoriesForCreateAsync()
        {
            var res = await _http.GetAsync($"/api/vendors/categories/create");
            res.EnsureSuccessStatusCode();

            return await res.Content
                .ReadFromJsonAsync<List<CategoryLookupDto>>() ?? new();
        }
        */

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


        public async Task<int> CreateProductAsync(VendorProductCreateDto dto)
        {
            var res = await _http.PostAsJsonAsync(
                "/api/vendors/products",
                dto);

            res.EnsureSuccessStatusCode();

            // API already returns int (NewProductID)
            return await res.Content.ReadFromJsonAsync<int>();
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
        public async Task<List<SelectListItem>> GetWarehousesForCreateAsync()
        {
            var res = await _http.GetAsync($"/api/vendors/warehouses/create");
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

        public async Task<List<SelectListItem>> GetVendorWarehousesAsync()
        {
            var res = await _http.GetAsync($"/api/vendors/warehouses/vendor");
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
        public async Task<VendorWarehouseDto> GetWarehousePreviewAsync(int warehouseId)
        {
            var res = await _http.GetAsync($"/api/vendors/warehouses/{warehouseId}");
            res.EnsureSuccessStatusCode();

            return await res.Content.ReadFromJsonAsync<VendorWarehouseDto>()
                   ?? throw new Exception("Warehouse not found");
        }

        /* =========================================================
           SUPPORT : RETURN POLICY LIST
           ========================================================= */
        public async Task<List<SelectListItem>> GetVendorReturnPolicyAsync()
        {
            var res = await _http.GetAsync("/api/vendors/return-policies");
            res.EnsureSuccessStatusCode();

            var data = await res.Content
                .ReadFromJsonAsync<List<ReturnPolicyDto>>() ?? [];

            return data.Select(r => new SelectListItem
            {
                Value = r.ReturnPolicyID.ToString(),
                Text = $"{r.PolicyName} ({r.ReturnDays} day{(r.ReturnDays > 1 ? "s" : "")})"
            }).ToList();
        }

        public async Task<List<SelectListItem>> GetCreateReturnPolicyAsync()
        {
            var res = await _http.GetAsync($"/api/vendors/return-policies/create");
            res.EnsureSuccessStatusCode();

            return await res.Content
                .ReadFromJsonAsync<List<SelectListItem>>() ?? new();

        }
        //=============================================================
        // SUPPORT : CATEGORY HSN PREVIEW

        public async Task<HsnLookupDto> GetCategoryHsnPreviewAsync(int categoryId)
        {
            var res = await _http.GetAsync($"/api/skufilter/{categoryId}/hsn");
            res.EnsureSuccessStatusCode();

            return await res.Content.ReadFromJsonAsync<HsnLookupDto>()
                   ?? throw new Exception("HSN not found");
        }

        
    }

}
