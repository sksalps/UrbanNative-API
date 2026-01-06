namespace UrbanNative.Application.GlobalCall.VariantValueSignature
{
    public static class ValueSignatureDecoder
    {
        public static string Decode(
            string? signature,
            Dictionary<int, string> variantNames,
            Dictionary<int, string> valueNames)
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

                if (!variantNames.TryGetValue(variantId, out var variantName)) continue;
                if (!valueNames.TryGetValue(valueId, out var valueName)) continue;

                readable.Add($"{variantName}: {valueName}");
            }

            return string.Join(", ", readable);
        }
    }
}
