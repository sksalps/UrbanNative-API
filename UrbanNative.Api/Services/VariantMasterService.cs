using UrbanNative.Domain.Entities;
using UrbanNative.Infrastructure.Repositories;

namespace UrbanNative.Api.Services
{
    public class VariantMasterService : IVariantMasterService
    {
        private readonly IVariantMasterRepository _repo;

        public VariantMasterService(IVariantMasterRepository repo)
        {
            _repo = repo;
        }

        public Task<int> AddVariantTypeAsync(string variantName)
        {
            return _repo.AddVariantMasterAsync(variantName);
        }

        public Task<IEnumerable<VariantMaster>> GetAllVariantTypesAsync()
        {
            return _repo.GetAllVariantTypesAsync();
        }
    }
}