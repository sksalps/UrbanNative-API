using UrbanNative.Application.DTOs.AdminCategory;
using UrbanNative.Application.DTOs.AdminVariant;

namespace UrbanNative.Application.Interfaces
{
    public interface IAdminVariantRepository
    {
        Task<IEnumerable<AdminVariantListDto>> GetVariantsAsync(
            string? search,
            bool? isActive);
        Task UpdateVariantValueAsync(int variantValueId, string valueName);

        Task<AdminVariantDetailsDto?> GetVariantByIdAsync(int variantId);

        Task<IEnumerable<AdminVariantValueDto>> GetVariantValuesAsync(int variantId);

        Task CreateVariantAsync(CreateVariantDto dto);

        Task UpdateVariantAsync(int variantId, string variantName);

        Task ToggleVariantStatusAsync(int variantId);

        Task CreateVariantValueAsync(CreateVariantValueDto dto);

        Task ToggleVariantValueStatusAsync(int variantValueId);
    }
}