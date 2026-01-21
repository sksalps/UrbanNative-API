using UrbanNative.Application.DTOs.Vendors.Products;

namespace UrbanNative.Vendors.Services.Interfaces
{
    public interface IVendorProductService
    {
        Task<List<VendorProductListDto>> GetMyProductsAsync();
    }
}
