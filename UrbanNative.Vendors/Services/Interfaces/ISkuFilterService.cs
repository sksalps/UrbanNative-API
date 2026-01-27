using UrbanNative.Application.DTOs.CommonCrossDashboard;

namespace UrbanNative.Vendors.Services.Interfaces
{
    public interface ISkuFilterService
    {
        Task<IReadOnlyList<SkuCategoryDto>> GetCategoriesAsync();
        Task<IReadOnlyList<SkuProductDto>> GetProductsAsync(int categoryId);
        Task<IReadOnlyList<SkuLookupDto>> SearchSkusAsync(int productId, string? q);
        Task<SkuContextDto?> GetSkuContextAsync(int skuId);
    }

}
