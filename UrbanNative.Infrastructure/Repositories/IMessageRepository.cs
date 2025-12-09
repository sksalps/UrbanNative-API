using System.Threading.Tasks;

namespace UrbanNative.Infrastructure.Repositories
{
    public interface IMessageRepository
    {
        Task<string> GetMessageAsync(string key, string language = "en");
    }
}


