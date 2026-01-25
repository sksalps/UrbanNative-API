using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Vendors.Products;

namespace UrbanNative.Application.Interfaces.UseCases
{
    public interface IVendorSkuUseCase
    {
        Task<List<VendorSkuGridDto>> GetGridAsync(int productId, int vendorId);
        Task SaveAsync(int vendorId, List<VendorSkuSaveDto> skus);
        Task<VendorSkuHeaderDto> GetHeaderAsync(int productId);
    }
}

