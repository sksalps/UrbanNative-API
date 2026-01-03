using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.GlobalCall.VariantValueSignature
{
    public class VariantSignatureDecoder : IVariantSignatureDecoder
    {
        private readonly VariantMasterCache _cache;

        public VariantSignatureDecoder(VariantMasterCache cache)
        {
            _cache = cache;
        }

        public string Decode(string? signature)
        {
            if (string.IsNullOrWhiteSpace(signature))
                return string.Empty;

            var parts = signature.Split('|', StringSplitOptions.RemoveEmptyEntries);
            var readable = new List<string>();

            foreach (var part in parts)
            {
                var ids = part.Split(':');
                if (ids.Length != 2) continue;

                if (!int.TryParse(ids[0], out var variantId)) continue;
                if (!int.TryParse(ids[1], out var valueId)) continue;

                if (!_cache.VariantNames.TryGetValue(variantId, out var variantName)) continue;
                if (!_cache.VariantValueNames.TryGetValue(valueId, out var valueName)) continue;

                readable.Add($"{variantName}: {valueName}");
            }

            return string.Join(", ", readable);
        }
    }

}
