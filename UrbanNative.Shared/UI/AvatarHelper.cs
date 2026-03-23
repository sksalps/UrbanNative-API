namespace UrbanNative.Shared.UI
{
    public static class AvatarHelper
    {
        public static string GetInitials(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "?";

            var parts = name
                .Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 1)
                return parts[0][0].ToString().ToUpper();

            return $"{parts[0][0]}{parts[1][0]}".ToUpper();
        }
    }
}
