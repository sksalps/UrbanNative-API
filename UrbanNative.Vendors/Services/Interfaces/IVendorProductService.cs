using UrbanNative.Application.DTOs.Vendors.Products;

namespace UrbanNative.Vendors.Services.Interfaces
{
    public interface IVendorProductService
    {
        Task<List<VendorProductListDto>> GetMyProductsAsync(
            string? search,
            int? categoryId,
            int? hsnId
        );

        Task<List<CategoryLookupDto>> GetVendorCategoriesAsync();

        Task<List<HsnLookupDto>> GetVendorHsnListAsync();
    }

}
