using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Vendors.Products;

namespace UrbanNative.Application.Interfaces.Vendors
{
    public interface IVendorProductRepository
    {
        Task<List<VendorProductListDto>> GetVendorProductsAsync(
            int vendorId,
            string? search,
            int? categoryId,
            int? hsnId
        ); 
        Task<List<CategoryLookupDto>> GetVendorCategoriesAsync(int? vendorId,bool? isActive);
        Task<List<HsnLookupDto>> GetVendorHsnListAsync();
        Task<int> CreateProductAsync(int vendorId, VendorProductCreateDto dto);
        Task UpdateProductAsync(int vendorId, VendorProductUpdateDto dto);
        Task<VendorProductEditDto> GetProductForEditAsync(int vendorId, int productId);
        //Task<List<VendorWarehouseDto>> GetVendorWarehousesAsync(int vendorId);
        Task<List<VendorWarehouseDto>> GetVendorWarehousesAsync( int? vendorId,    bool? isActive);
        Task<List<ReturnPolicyDto>> GetReturnPoliciesAsync( bool? isActive);
    }

}
