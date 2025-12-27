using UrbanNative.Application.DTOs.AdminSKU;

namespace UrbanNative.Infrastructure.Repository
{
    internal static class SkuCombinationGenerator
    {
        public static List<SkuSignatureDto> Generate(
            List<VariantSelectionDto> selections)
        {
            var ordered = selections
                .OrderBy(v => v.VariantId)
                .ToList();

            var combinations = new List<string> { string.Empty };

            foreach (var variant in ordered)
            {
                var next = new List<string>();

                foreach (var combo in combinations)
                {
                    foreach (var valueId in variant.VariantValueIds)
                    {
                        next.Add($"{combo}{variant.VariantId}:{valueId}|");
                    }
                }

                combinations = next;
            }

            return combinations
                .Select((sig, index) => new SkuSignatureDto
                {
                    SeqNo = index + 1,
                    ValueSignature = sig
                })
                .ToList();
        }
    }
}
