using System;
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
        Task<List<CategoryLookupDto>> GetVendorCategoriesAsync(int vendorId);
        Task<List<HsnLookupDto>> GetVendorHsnListAsync();
    }
}
