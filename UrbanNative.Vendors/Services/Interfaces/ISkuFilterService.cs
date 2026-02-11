using Microsoft.AspNetCore.Mvc.Rendering;
using UrbanNative.Application.DTOs.AdminReturnsOrder;
using UrbanNative.Application.DTOs.CommonCrossDashboard;
using UrbanNative.Application.DTOs.Vendors.Products;

namespace UrbanNative.Vendors.Services.Interfaces
{
    public interface ISkuFilterService
    {
        Task<IReadOnlyList<LogisticsProviderComnDto>> GetLogisticsProvidersAsync();
        Task<IReadOnlyList<SkuCategoryDto>> GetCategoriesAsync();
        Task<IReadOnlyList<SkuCategoryDto>> GetCategoriesForFilter();
        Task<IReadOnlyList<SkuProductDto>> GetProductsAsync(int categoryId);
        Task<SkuProductBasicDto?> GetProductByIdAsync(int productId);
        Task<IReadOnlyList<SkuLookupDto>> SearchSkusAsync(int productId, string? search);
        Task<SkuContextDto?> GetSkuContextAsync(int skuId);
        Task<IReadOnlyList<CommonWarehouseDto>> GetAllWarehousesAsync();// For dropdowns in filter active/inactive
        Task<IReadOnlyList<CommonWarehouseDto>> GetActiveWarehouseAsync();// Only Active for dropdowns in create product
        Task<WarehousePreviewDto> GetWarehousePreviewAsync(int addresId); //Get One particular warehouse by ID
        Task<IReadOnlyList<CommonWarehouseDto>> GetVendorActiveWarehouseAsync();// Get All Active Warehouses for Vendor
    }

}
