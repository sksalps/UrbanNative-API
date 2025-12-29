using System.Data;
using UrbanNative.Application.DTOs.AdminSKU;

namespace UrbanNative.Application.Interfaces
{
    public interface IAdminSkuRepository
    {
        // 1️⃣ SKU generation (used by AdminProductSKUController)
        Task GenerateAndSaveSkusAsync(int productId, List<VariantSelectionDto> selections,decimal price, int stock,int? returnPolicyId );

        // 2️⃣ SKU overview (used by Razor Admin UI)
        Task<List<AdminSkuOverviewDto>> GetSkuOverviewAsync();
        Task<List<ProductSkuDetailDto>> GetProductSkusAsync(int productId);
        Task<List<AdminVendorSkuCoverageDto>> GetVendorCoverageAsync(int productId, bool includeInactiveVendors);

        Task<ProductSkuCoverageHeaderDto> GetCoverageHeaderAsync(int productId);
        Task<List<VendorSkuCoverageDetailDto>> GetVendorCoverageDetailAsync(int productId,  int vendorId       );







    }
}
