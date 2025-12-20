using UrbanNative.Application.DTOs.AdminCategory;

namespace UrbanNative.Application.Interfaces
{
    public interface IAdminCategoryRepository
    {
        // =========================
        // Admin – Category Listing
        // =========================
        Task<IEnumerable<AdminCategoryListDto>> GetAdminCategoriesAsync();

        // =========================
        // Admin – Category Details
        // =========================
        Task<AdminCategoryDetailDto?> GetAdminCategoryByIdAsync(int categoryId);

        // =========================
        // Admin – Create / Update
        // =========================
        Task CreateCategoryAsync(AdminCategorySaveDto dto, int adminUserId);
        Task<bool> UpdateCategoryAsync(int categoryId, AdminCategorySaveDto dto, int adminUserId);

        // =========================
        // Admin – Activate / Deactivate
        // =========================
        Task<(bool Success, string Message)> ToggleCategoryActiveAsync(int categoryId);
    }
}
