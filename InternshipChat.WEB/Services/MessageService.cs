using Blazored.LocalStorage;
using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO.ChatDtos;
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

        public async Task<List<MessageDTO>> GetMessagesAsync(int chatId)
        {
            await GetBearerToken();
            return await _httpClient.GetFromJsonAsync<List<MessageDTO>>($"api/message/{chatId}");
        }

        public async Task SaveMessageAsync(MessageDTO messageDTO)
        {
            var newMessage = new CreateMessageDTO()
            {
                ChatId = messageDTO.ChatId,
                UserId = messageDTO.UserId,
                MessageContent = messageDTO.MessageContent
            };

            await GetBearerToken();
            await _httpClient.PostAsJsonAsync("api/message", newMessage);
        }
    }
}
