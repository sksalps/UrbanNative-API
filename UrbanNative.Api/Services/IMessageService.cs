using System.Threading.Tasks;

namespace UrbanNative.Api.Services
{
    public interface IMessageService
    {
        Task<string> GetMessage(string key, string lang = "en");
    }
}
