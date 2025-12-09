using UrbanNative.Domain.Entities;

namespace UrbanNative.Infrastructure.Repositories
{
    public interface IVariantMasterRepository
    {
        Task<int> AddVariantMasterAsync(string variantName);
        Task<IEnumerable<VariantMaster>> GetAllVariantTypesAsync();
    }
}