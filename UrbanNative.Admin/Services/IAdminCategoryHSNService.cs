using UrbanNative.Application.DTOs.AdminCategory;

namespace UrbanNative.Admin.Services
{
    public interface IAdminCategoryHSNService
    {
        Task<CategoryHSNDto?> GetByCategoryIdAsync(int categoryId);

        Task LinkOrUpdateAsync(int categoryId, int hsnId);

        Task RemoveAsync(int categoryId);
    }
}