using UrbanNative.Application.DTOs.AdminSKU;

namespace UrbanNative.Application.Interfaces
{
    public interface IAdminSkuRepository
    {
        Task GenerateAndSaveSkusAsync(
            int productId,
            List<VariantSelectionDto> selections,
            decimal price,
            int stock,
            int? returnPolicyId
        );
    }
}
