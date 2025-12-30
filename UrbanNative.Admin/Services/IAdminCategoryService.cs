using UrbanNative.Application.DTOs.AdminCategory;

namespace UrbanNative.Admin.Services
{
    public interface IAdminCategoryService
    {
        // =========================
        // Admin – Category Listing
        // =========================
        Task<IEnumerable<AdminCategoryListDto>> GetCategoriesAsync();

        // =========================
        // Admin – Single Category
        // =========================
        Task<AdminCategoryDetailDto?> GetByIdAsync(int categoryId);

        // =========================
        // Admin – Create / Update
        // =========================
        Task<string?> CreateAsync(AdminCategorySaveDto dto);

        Task<string?> UpdateAsync(int categoryId, AdminCategorySaveDto dto);


        Task<List<CategoryBreadcrumbDto>> GetBreadcrumbAsync(int categoryId);

        Task<AdminCategoryDetailsDto> GetCategoryDetailsAsync(int categoryId);
        Task<List<AdminCategoryFlatDto>> GetFlatCategoriesAsync();

        // =========================
        // Admin – Activate / Deactivate
        // =========================
        Task<string?> ToggleActiveAsync(int categoryId);

    }

}
