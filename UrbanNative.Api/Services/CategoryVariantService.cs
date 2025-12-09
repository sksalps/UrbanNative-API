using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Repositories;

namespace UrbanNative.Api.Services
{
    public class CategoryVariantService : ICategoryVariantService
    {
        private readonly ICategoryVariantRepository _repo;

        public CategoryVariantService(ICategoryVariantRepository repo)
        {
            _repo = repo;
        }

        public Task<int> AssignVariantToCategoryAsync(int categoryId, int variantId, int adminId)
            => _repo.AssignVariantToCategoryAsync(categoryId, variantId, adminId);

        public Task<bool> RemoveVariantFromCategoryAsync(int categoryId, int variantId)
            => _repo.RemoveVariantFromCategoryAsync(categoryId, variantId);

        public Task<IEnumerable<VariantMaster>> GetVariantsByCategoryAsync(int categoryId)
            => _repo.GetVariantsByCategoryAsync(categoryId);
    }
}