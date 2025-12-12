using UrbanNative.Domain.Entities;

namespace UrbanNative.Api.Services
{
    public interface IProductVariantSetService
    {
        Task<int> AddVariantSetAsync(ProductVariantSet set);
        Task<IEnumerable<ProductVariantSet>> GetVariantSetsByProductAsync(int productId);
    }
}