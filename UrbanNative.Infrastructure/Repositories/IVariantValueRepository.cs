using UrbanNative.Domain.Entities;

namespace UrbanNative.Infrastructure.Repositories
{
    public interface IVariantValueRepository
    {
        Task<int> AddVariantValueAsync(int variantId, string valueName);
        Task<IEnumerable<VariantValue>> GetValuesByVariantAsync(int variantId);
    }
}