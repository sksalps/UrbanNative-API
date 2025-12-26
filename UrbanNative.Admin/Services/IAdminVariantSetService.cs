using UrbanNative.Application.DTOs.AdminCategory;
using UrbanNative.Application.DTOs.AdminVariantSet;

namespace UrbanNative.Admin.Services
{
    public interface IAdminVariantSetService
{
    Task<IEnumerable<AdminVariantSetListDto>> GetAllAsync();
    Task<AdminVariantSetDetailsDto?> GetDetailsAsync(int variantSetId);
    Task<IEnumerable<AdminCategoryListDto>> GetAvailableCategoriesAsync(int variantSetId);

        Task MoveVariantAsync(int variantSetVariantId, string direction);

    Task CreateAsync(string variantSetName);
    Task UpdateAsync(int variantSetId, string variantSetName);
    Task ToggleStatusAsync(int variantSetId);

    Task<IEnumerable<AdminVariantInsideSetDto>> GetVariantsAsync(int variantSetId);
    Task AddVariantAsync(int variantSetId, int variantId);
    Task RemoveVariantAsync(int variantSetVariantId);
    Task UpdateVariantOrderAsync(int variantSetVariantId, int newSortOrder);

    Task<IEnumerable<AdminVariantSetCategoryDto>> GetAssignedCategoriesAsync(int variantSetId);
    Task AssignCategoryAsync(int variantSetId, int categoryId);
    Task RemoveCategoryAsync(int categoryVariantSetId);
}
}