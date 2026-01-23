using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Vendors.Products;

namespace UrbanNative.Application.Interfaces.UseCases
{
    public interface IVendorProductsUseCase
    {
        //Task<List<VendorProductListDto>> ExecuteAsync(int vendorId);
        Task<List<VendorProductListDto>> ExecuteAsync( int vendorId,   string? search,    int? categoryId,    int? hsnId);
        Task<List<CategoryLookupDto>> GetVendorCategoriesAsync(int? vendorId, bool? isActive);
        Task<List<HsnLookupDto>> GetVendorHsnListAsync();
        Task<int> ExecuteAsync(int vendorId, VendorProductCreateDto dto);
        Task ExecuteAsync(int vendorId, VendorProductUpdateDto dto);
        Task<VendorProductEditDto> ExecuteAsync(int vendorId, int productId);
        Task<List<VendorWarehouseDto>> ExecuteAsync(int? vendorId, bool? isActive);
        Task<List<ReturnPolicyDto>> GetReturnPoliciesAsync(bool? isActive);
    }
}
