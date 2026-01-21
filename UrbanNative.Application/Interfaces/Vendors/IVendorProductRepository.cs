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
        Task<List<VendorProductListDto>> GetVendorProductsAsync(int vendorId);

    }
}
