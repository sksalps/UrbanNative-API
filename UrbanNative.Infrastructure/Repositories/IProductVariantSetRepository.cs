using UrbanNative.Domain.Entities;

namespace UrbanNative.Infrastructure.Repositories
{
    public interface IProductVariantSetRepository
    {
        Task<int> AddVariantSetAsync(ProductVariantSet set);
        Task<IEnumerable<ProductVariantSet>> GetVariantSetsByProductAsync(int productId);
    }
}