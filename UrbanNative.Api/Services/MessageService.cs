using UrbanNative.Infrastructure.Repositories;

namespace UrbanNative.Api.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _repo;

        public MessageService(IMessageRepository repo)
        {
            _repo = repo;
        }

        public Task<string> GetMessage(string key, string lang = "en")
        {
            return _repo.GetMessageAsync(key, lang);

        }
        public async Task<string> GetMessageAsync(string messageKey, string lang="en")
        {
            return await _repo.GetMessageAsync(messageKey);
        }
    }
}
