using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using UrbanNative.Application.DTOs.CommonCrossDashboard;
using UrbanNative.Vendors.Services.Interfaces;
using static System.Net.WebRequestMethods;

namespace UrbanNative.Vendors.Services
{
    public class SkuFilterService : ISkuFilterService
    {
        private readonly HttpClient _http;

        public SkuFilterService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }
        
        //  Fetch Only active Logistics Provider for assigning shipment of orders
        public async Task<IReadOnlyList<LogisticsProviderComnDto>> GetLogisticsProvidersAsync()
            => await _http.GetFromJsonAsync<List<LogisticsProviderComnDto>>(
                    "api/skufilter/logisticprovider") ?? new();


        //Fetch all Categories Active|Inactive 
        public async Task<IReadOnlyList<SkuCategoryDto>> GetCategoriesAsync()
            => await _http.GetFromJsonAsync<List<SkuCategoryDto>>(
                "api/skufilter/categories") ?? new();

        // Get Category of a vendor, Active/Inactive Status for filter dropdown
        public async Task<IReadOnlyList<SkuCategoryDto>> GetCategoriesForFilter()
            => await _http.GetFromJsonAsync<List<SkuCategoryDto>>(
                "api/skufilter/categories/vendor") ?? new();

        // ====================================================== 
        // ✅ CORRECT SKU CONTEXT Product CALL 
        // ======================================================
        public async Task<SkuContextDto?> GetSkuContextAsync(int skuId)
            => await _http.GetFromJsonAsync<SkuContextDto>(
                $"api/skufilter/skus/{skuId}/context");
        public async Task<IReadOnlyList<SkuProductDto>> GetProductsAsync(int categoryId)
            => await _http.GetFromJsonAsync<List<SkuProductDto>>(
                $"api/skufilter/products?categoryId={categoryId}") ?? new();

        public async Task<SkuProductBasicDto?> GetProductByIdAsync(int productId)
        {
            var response = await _http.GetAsync(
                $"api/skufilter/product/{productId}");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content
                .ReadFromJsonAsync<SkuProductBasicDto>();
        }
        public async Task<IReadOnlyList<SkuLookupDto>> SearchSkusAsync(int productId, string? search)
            => await _http.GetFromJsonAsync<List<SkuLookupDto>>(
                $"api/skufilter/skus?productId={productId}&q={search}") ?? new();

        // ====================================================== 
        // ✅ Warehouse Lookup Calls
        // ======================================================

        //Get all Active warehouse of a vendor for add inventory in active warehouses only
        public async Task<IReadOnlyList<CommonWarehouseDto>> GetVendorActiveWarehouseAsync()
            => await _http.GetFromJsonAsync<List<CommonWarehouseDto>>(
                    "api/skufilter/warehouses/vendoractive") ?? new();

        //  Fetch Only active warehouses for creating/editing products
        public async Task<IReadOnlyList<CommonWarehouseDto>> GetActiveWarehouseAsync()
            => await _http.GetFromJsonAsync<List<CommonWarehouseDto>>(
                    "api/skufilter/warehouses/create") ?? new ();
        
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
