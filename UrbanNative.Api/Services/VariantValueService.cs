using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Repositories;

namespace UrbanNative.Api.Services
{
    public class VariantValueService : IVariantValueService
    {
        private readonly IVariantValueRepository _repo;

        public VariantValueService(IVariantValueRepository repo)
        {
            _repo = repo;
        }

        public Task<int> AddVariantValueAsync(int variantId, string valueName)
        {
            return _repo.AddVariantValueAsync(variantId, valueName);
        }

        public Task<IEnumerable<VariantValue>> GetValuesByVariantAsync(int variantId)
        {
            return _repo.GetValuesByVariantAsync(variantId);
        }
    }
}