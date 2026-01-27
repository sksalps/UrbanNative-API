using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanNative.Application.DTOs.CommonCrossDashboard;

namespace UrbanNative.Application.Interfaces.CommonCrossDashboard
{
    public interface ISkuFilterRepository
    {
        Task<IReadOnlyList<SkuCategoryDto>> GetCategoriesAsync(
            int? vendorId,
            bool? isActive);

        Task<IReadOnlyList<SkuProductDto>> GetProductsAsync(
            int? vendorId,
            int categoryId);

        // ✅ FIXED SIGNATURE
        Task<IReadOnlyList<SkuLookupDto>> SearchSkusAsync(
            int? vendorId,
            int productId,
            string? search);

        Task<SkuContextDto?> GetSkuContextAsync(
            int skuId,
            int? vendorId);
        Task<HsnLookupDto> GetHsnByCategoryAsync(int categoryId);
    }
}
