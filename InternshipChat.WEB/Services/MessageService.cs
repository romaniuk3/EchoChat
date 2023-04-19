using Blazored.LocalStorage;
using InternshipChat.DAL.Entities;
using InternshipChat.WEB.Services.Base;
using InternshipChat.WEB.Services.Contracts;

namespace InternshipChat.WEB.Services
{
    public class MessageService : BaseHttpService, IMessageService
    {
        private readonly HttpClient _httpClient;

        public MessageService(HttpClient httpClient, ILocalStorageService localStorage) : base(httpClient, localStorage)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Message>> GetMessages()
        {
            await GetBearerToken();
            return await _httpClient.GetFromJsonAsync<List<Message>>("api/Chat/GetMessages");
        }
    }
}
