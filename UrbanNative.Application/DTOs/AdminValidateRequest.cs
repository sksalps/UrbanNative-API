

/// <summary>
/// Request payload for admin credential validation.
/// Identifier = username OR email.
/// </summary>
/// 
namespace UrbanNative.Application.DTOs
{ 

public class AdminValidateRequest
{
        public string Identifier { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}