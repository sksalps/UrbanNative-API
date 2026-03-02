using System.Text.RegularExpressions;

namespace UrbanNative.Infrastructure.Services
{
    public static class OcrParser
    {
        public static string? ExtractPan(string text)
        {
            var match = Regex.Match(text, @"[A-Z]{5}[0-9]{4}[A-Z]");
            return match.Success ? match.Value : null;
        }
    }
}