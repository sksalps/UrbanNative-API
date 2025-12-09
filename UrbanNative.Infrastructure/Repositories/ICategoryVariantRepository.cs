using UrbanNative.Domain.Entities;

namespace UrbanNative.Infrastructure.Repositories
{
    public interface ICategoryVariantRepository
    {
        Task<int> AssignVariantToCategoryAsync(int categoryId, int variantId, int createdBy);
        Task<bool> RemoveVariantFromCategoryAsync(int categoryId, int variantId);
        Task<IEnumerable<VariantMaster>> GetVariantsByCategoryAsync(int categoryId);
    }
}