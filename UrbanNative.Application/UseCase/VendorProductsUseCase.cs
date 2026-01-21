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

        public async Task<List<VendorProductListDto>> ExecuteAsync(int vendorId)
        {
            return await _repo.GetVendorProductsAsync(vendorId);
        }
    }

}
