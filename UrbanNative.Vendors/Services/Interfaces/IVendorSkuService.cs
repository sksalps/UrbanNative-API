using Microsoft.AspNetCore.Mvc.Rendering;

using UrbanNative.Application.DTOs.Vendors.Products;

namespace UrbanNative.Vendors.Services.Interfaces
{
    public interface IVendorSkuService
    {
        Task<VendorSkuHeaderDto> GetHeaderAsync(int productId);
        Task<List<VendorSkuGridDto>> GetGridAsync(int productId);
        Task SaveAsync(int productId, List<VendorSkuSaveDto> skus);
    }
       
}
