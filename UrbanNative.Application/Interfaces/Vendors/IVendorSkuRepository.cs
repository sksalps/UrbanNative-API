using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.Vendors.Products;

namespace UrbanNative.Application.Interfaces.Vendors
{
    public interface IVendorSkuRepository
    {
        Task<List<VendorSkuGridDto>> GetSkuGridAsync(int productId, int vendorId);
        Task SaveSkusAsync(int vendorId, List<VendorSkuSaveDto> skus);
        Task<VendorSkuHeaderDto> GetSkuHeaderAsync(int productId);
    }
}
