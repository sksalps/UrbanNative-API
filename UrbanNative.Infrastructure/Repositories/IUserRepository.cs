using UrbanNative.Domain.Entities;


namespace UrbanNative.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<int> RegisterAsync(User user);
        Task<User?> LoginAsync(string mobile);
    }
}


