using UrbanNative.Application.DTOs.AdminCategory;

namespace UrbanNative.Application.Interfaces
{
    public interface IAdminCategoryHSNRepository
    {
        Task<CategoryHSNDto?> GetByCategoryIdAsync(int categoryId);

        Task LinkOrUpdateAsync(CategoryHSNLinkDto dto);

        Task RemoveAsync(int categoryId, int adminId);
    }
}