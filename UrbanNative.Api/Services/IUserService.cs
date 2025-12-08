using UrbanNative.Domain.Entities;

namespace UrbanNative.Api.Services
{
       public interface IUserService
    {
        Task<int> RegisterAsync(User user);
        Task<User?> LoginAsync(string mobile);
        string GenerateJwtToken(User user);
    }
}
