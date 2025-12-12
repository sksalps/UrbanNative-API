
using UrbanNative.Domain.Entities;

namespace UrbanNative.Api.Services
{
    public interface IVariantMasterService
    {
        Task<int> AddVariantTypeAsync(string variantName);
        Task<IEnumerable<VariantMaster>> GetAllVariantTypesAsync();
    }
}