using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Vendors.Products;
using UrbanNative.Application.Interfaces.UseCases;
using UrbanNative.Application.Interfaces.Vendors;

namespace UrbanNative.Application.UseCase
{
    public class VendorSkuUseCase : IVendorSkuUseCase
    {
        private readonly IVendorSkuRepository _repo;

        public VendorSkuUseCase(IVendorSkuRepository repo)
        {
            _repo = repo;
        }

        public Task<List<VendorSkuGridDto>> GetGridAsync(int productId, int vendorId)
            => _repo.GetSkuGridAsync(productId, vendorId);

        public Task SaveAsync(int vendorId, List<VendorSkuSaveDto> skus) 
            => _repo.SaveSkusAsync(vendorId, skus);
        public Task<VendorSkuHeaderDto> GetHeaderAsync(int productId)
            => _repo.GetSkuHeaderAsync(productId);
    }
}
