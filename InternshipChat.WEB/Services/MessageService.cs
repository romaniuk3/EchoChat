using InternshipChat.DAL.Entities;
using InternshipChat.WEB.Services.Contracts;

namespace InternshipChat.WEB.Services
{
    public class MessageService : IMessageService
    {
        private readonly HttpClient _httpClient;

        public MessageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Message>> GetMessages()
        {
            return await _httpClient.GetFromJsonAsync<List<Message>>("Chat/GetMessages");
        }
    }
}
