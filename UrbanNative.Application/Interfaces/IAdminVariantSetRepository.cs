using UrbanNative.Application.DTOs.AdminCategory;
using UrbanNative.Application.DTOs.AdminVariantSet;

namespace UrbanNative.Application.Interfaces
{

    public interface IAdminVariantSetRepository
    {
        Task<IEnumerable<AdminVariantSetListDto>> GetAllAsync();
        Task<AdminVariantSetDetailsDto?> GetByIdAsync(int variantSetId);

        Task CreateAsync(string variantSetName);
        Task UpdateAsync(int variantSetId, string variantSetName);
        Task ToggleStatusAsync(int variantSetId);
        Task<IEnumerable<AdminCategoryListDto>> GetAvailableCategoriesAsync(int variantSetId);

        Task<IEnumerable<AdminVariantInsideSetDto>> GetVariantsBySetIdAsync(int variantSetId);
        Task AddVariantToSetAsync(int variantSetId, int variantId);
        Task RemoveVariantFromSetAsync(int variantSetVariantId);

        Task MoveVariantAsync(int variantSetVariantId, string direction);

        Task UpdateVariantOrderAsync(int variantSetVariantId, int newSortOrder);
        Task<IEnumerable<AdminVariantSetCategoryDto>> GetCategoriesBySetIdAsync(int variantSetId);
        Task AssignCategoryAsync(int variantSetId, int categoryId);
        Task RemoveCategoryAsync(int categoryVariantSetId);
    }

}