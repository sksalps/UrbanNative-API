using UrbanNative.Application.DTOs.CommonCrossDashboard;
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

        public async Task<IReadOnlyList<SkuLookupDto>> SearchSkusAsync(int productId, string? q)
            => await _http.GetFromJsonAsync<List<SkuLookupDto>>(
                $"api/skufilter/skus?productId={productId}&q={q}") ?? new();
    }

}
