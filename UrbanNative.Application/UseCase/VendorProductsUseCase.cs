using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.Interfaces.UseCases;
using UrbanNative.Application.Interfaces.Vendors;
using UrbanNative.Application.DTOs.Vendors.Products;

namespace UrbanNative.Application.UseCase
{
    public class VendorProductsUseCase : IVendorProductsUseCase
    {
        private readonly IVendorProductRepository _repo;

        public VendorProductsUseCase(IVendorProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<VendorProductListDto>> ExecuteAsync(int vendorId, string? search, int? categoryId, int? hsnId)
        {
            return await _repo.GetVendorProductsAsync(vendorId, search, categoryId, hsnId);
        }

        public Task<List<CategoryLookupDto>> GetVendorCategoriesAsync(int vendorId)
        => _repo.GetVendorCategoriesAsync(vendorId);

        public Task<List<HsnLookupDto>> GetVendorHsnListAsync()
            => _repo.GetVendorHsnListAsync();
    }

}
