using UrbanNative.Application.DTOs.AdminVariant;

namespace UrbanNative.Admin.Services
{
    public interface IAdminVariantService
    {
        Task<IEnumerable<AdminVariantListDto>> GetVariantsAsync(string? search, bool? isActive);
        Task<AdminVariantDetailsDto> GetVariantAsync(int variantId);

        Task CreateVariantAsync(string variantName);
        Task UpdateVariantAsync(int variantId, string variantName);   // ✅ ADDED
        Task ToggleVariantAsync(int variantId);

        Task<IEnumerable<AdminVariantValueDto>> GetVariantValuesAsync(int variantId);
        Task AddVariantValueAsync(int variantId, string valueName);
        Task ToggleVariantValueAsync(int variantValueId);
    }

}

