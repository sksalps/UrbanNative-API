using UrbanNative.Domain.Entities;

namespace UrbanNative.Infrastructure.Repositories
{
    public interface IProductVariantValuesRepository
    {
        Task<bool> AddValueToVariantSetAsync(int variantSetId, int valueId);
        Task<IEnumerable<ProductVariantValues>> GetValuesByVariantSetAsync(int variantSetId);
    }
}
