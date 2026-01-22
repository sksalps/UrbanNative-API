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

        Task<List<CategoryLookupDto>> GetVendorCategoriesAsync();
        Task<List<HsnLookupDto>> GetVendorHsnListAsync();

        /* ---------- CREATE (Step-2.1) ---------- */
        Task CreateProductAsync(VendorProductCreateDto dto);

        /* ---------- EDIT (Step-2.2) ---------- */
        Task<VendorProductEditDto> GetProductForEditAsync(int productId);
        Task UpdateProductAsync(VendorProductUpdateDto dto);

        /* ---------- SUPPORT ---------- */
        Task<List<SelectListItem>> GetVendorWarehousesAsync();
    }


}
