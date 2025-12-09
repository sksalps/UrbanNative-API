using UrbanNative.Domain.Entities;

namespace UrbanNative.Api.Services
{
    public interface ICategoryVariantService
    {
        Task<int> AssignVariantToCategoryAsync(int categoryId, int variantId, int adminId);
        Task<bool> RemoveVariantFromCategoryAsync(int categoryId, int variantId);
        Task<IEnumerable<VariantMaster>> GetVariantsByCategoryAsync(int categoryId);
    }
}
