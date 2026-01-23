using Microsoft.AspNetCore.Mvc.Rendering;
using UrbanNative.Application.DTOs.Vendors.Products;

namespace UrbanNative.Vendors.Services.Interfaces
{
    public interface IVendorProductService
    {
        /* ---------- LIST (Step-1) ---------- */
        Task<List<VendorProductListDto>> GetMyProductsAsync(
            string? search,
            int? categoryId,
            int? hsnId);

        Task<List<CategoryLookupDto>> GetVendorActiveCategoriesAsync();
        Task<List<CategoryLookupDto>> GetVendorAllCategoriesAsync();
        Task<List<HsnLookupDto>> GetVendorHsnListAsync();

        /* ---------- CREATE (Step-2.1) ---------- */
        Task CreateProductAsync(VendorProductCreateDto dto);

        /* ---------- EDIT (Step-2.2) ---------- */
        Task<VendorProductEditDto> GetProductForEditAsync(int productId);
        Task UpdateProductAsync(VendorProductUpdateDto dto);

        /* ---------- SUPPORT ---------- */
        Task<List<SelectListItem>> GetVendorWarehousesAsync(); 
        Task<List<SelectListItem>> GetWarehousesForCreateAsync();
        Task<List<SelectListItem>> GetVendorReturnPolicyAsync();
        Task<List<SelectListItem>> GetCreateReturnPolicyAsync();

        Task<HsnLookupDto> GetCategoryHsnPreviewAsync(int categoryId);
        Task<VendorWarehouseDto> GetWarehousePreviewAsync(int warehouseId);
    }


}
