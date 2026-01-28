using Microsoft.AspNetCore.Mvc.Rendering;
using UrbanNative.Application.DTOs.CommonCrossDashboard;
using UrbanNative.Application.DTOs.Vendors.Products;
using UrbanNative.Vendors.Services.Interfaces;

namespace UrbanNative.Vendors.Services
{
    public class SkuFilterService : ISkuFilterService
    {
        private readonly HttpClient _http;

        public SkuFilterService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        } 

        public async Task<IReadOnlyList<SkuCategoryDto>> GetCategoriesAsync()
            => await _http.GetFromJsonAsync<List<SkuCategoryDto>>(
                "api/skufilter/categories") ?? new();
        // ======================================================
        // ✅ CORRECT SKU CONTEXT CALL
        // ======================================================
        public async Task<SkuContextDto?> GetSkuContextAsync(int skuId)
            => await _http.GetFromJsonAsync<SkuContextDto>(
                $"api/skufilter/skus/{skuId}/context");
        public async Task<IReadOnlyList<SkuProductDto>> GetProductsAsync(int categoryId)
            => await _http.GetFromJsonAsync<List<SkuProductDto>>(
                $"api/skufilter/products?categoryId={categoryId}") ?? new();

        public async Task<IReadOnlyList<SkuLookupDto>> SearchSkusAsync(int productId, string? search)
            => await _http.GetFromJsonAsync<List<SkuLookupDto>>(
                $"api/skufilter/skus?productId={productId}&q={search}") ?? new();


        //Get all Active warehouse of a vendor for create
        public async Task<List<SelectListItem>> GetActiveWarehouseAsync()
        {
            var res = await _http.GetAsync($"/api/skufilter/warehouses/active");
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
        //Get all Warehouses of a vendor for filter dropdown active/inactive both
        public async Task<IReadOnlyList<CommonWarehouseDto>> GetAllWarehousesAsync()
                    => await _http.GetFromJsonAsync<List<CommonWarehouseDto>>(
                "api/skufilter/warehouses/vendor") ?? new();
        //Get particular warehouse by ID for preview
        
        public async Task<WarehousePreviewDto> GetWarehousePreviewAsync(int warehouseId)
        {
            var url = $"api/skufilter/warehouse/{warehouseId}";
            return await _http.GetFromJsonAsync<WarehousePreviewDto>(url)
                   ?? throw new Exception("Warehouse preview not found");
        }
    } 

    }
