namespace UrbanNative.Api.Models
{
    /// <summary>
    /// Request payload for admin credential validation.
    /// Identifier = username OR email.
    /// </summary>
    public class AdminValidateRequest
    {
        public string Identifier { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}