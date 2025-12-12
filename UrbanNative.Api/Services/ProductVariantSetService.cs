using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Repositories;

namespace UrbanNative.Api.Services
{
    public class ProductVariantSetService : IProductVariantSetService
    {
        private readonly IProductVariantSetRepository _repo;

        public ProductVariantSetService(IProductVariantSetRepository repo)
        {
            _repo = repo;
        }

        public Task<int> AddVariantSetAsync(ProductVariantSet set)
        {
            return _repo.AddVariantSetAsync(set);
        }

        public Task<IEnumerable<ProductVariantSet>> GetVariantSetsByProductAsync(int productId)
        {
            return _repo.GetVariantSetsByProductAsync(productId);
        }
    }
}