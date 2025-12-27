using UrbanNative.Application.DTOs.AdminSKU;
namespace UrbanNative.Admin.Services
{
    public interface ISkuCombinationGenerator
    {
        List<SkuSignatureDto> Generate(List<VariantSelectionDto> selections);
    }
}
