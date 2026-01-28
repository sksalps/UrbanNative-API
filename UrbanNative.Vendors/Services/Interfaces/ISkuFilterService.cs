using Microsoft.AspNetCore.Mvc.Rendering;
using UrbanNative.Application.DTOs.CommonCrossDashboard;

namespace UrbanNative.Vendors.Services.Interfaces
{
    public interface ISkuFilterService
    {
        Task<IReadOnlyList<SkuCategoryDto>> GetCategoriesAsync();
        Task<IReadOnlyList<SkuProductDto>> GetProductsAsync(int categoryId);
        Task<IReadOnlyList<SkuLookupDto>> SearchSkusAsync(int productId, string? search);
        Task<SkuContextDto?> GetSkuContextAsync(int skuId);
        Task<IReadOnlyList<CommonWarehouseDto>> GetAllWarehousesAsync();// For dropdowns in filter active/inactive
        Task<List<SelectListItem>> GetActiveWarehouseAsync();// Only Active for dropdowns in create product
        Task<WarehousePreviewDto> GetWarehousePreviewAsync(int addresId); //Get One particular warehouse by ID
    }

}
