using UrbanNative.Domain.Entities;

namespace UrbanNative.Api.Services
{
    public interface IProductVariantValuesService
    {
        Task<bool> AddValueToVariantSetAsync(int variantSetId, int valueId);
        Task<IEnumerable<ProductVariantValues>> GetValuesByVariantSetAsync(int variantSetId);
    }
}