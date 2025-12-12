using UrbanNative.Domain.Entities;

namespace UrbanNative.Api.Services
{
    public interface IVariantValueService
    {
        Task<int> AddVariantValueAsync(int variantId, string valueName);
        Task<IEnumerable<VariantValue>> GetValuesByVariantAsync(int variantId);
    }
}