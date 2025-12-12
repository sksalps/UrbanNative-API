using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Repositories;

namespace UrbanNative.Api.Services
{
    public class ProductVariantValuesService : IProductVariantValuesService
    {
        private readonly IProductVariantValuesRepository _repo;

        public ProductVariantValuesService(IProductVariantValuesRepository repo)
        {
            _repo = repo;
        }

        public Task<bool> AddValueToVariantSetAsync(int variantSetId, int valueId)
        {
            return _repo.AddValueToVariantSetAsync(variantSetId, valueId);
        }

        public Task<IEnumerable<ProductVariantValues>> GetValuesByVariantSetAsync(int variantSetId)
        {
            return _repo.GetValuesByVariantSetAsync(variantSetId);
        }
    }
}