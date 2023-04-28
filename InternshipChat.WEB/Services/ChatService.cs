using Blazored.LocalStorage;
using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO.ChatDtos;
using InternshipChat.WEB.Services.Base;
using InternshipChat.WEB.Services.Contracts;
using System.Text.Json;

namespace InternshipChat.WEB.Services
{
    public class ChatService : BaseHttpService, IChatService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;

        public ChatService(HttpClient httpClient, ILocalStorageService localStorage) : base(httpClient, localStorage)
        {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task CreateChat(CreateChatDTO createChatDTO)
        {
            await GetBearerToken();
            await _httpClient.PostAsJsonAsync("api/chat/create", createChatDTO, _options);
        }

        public async Task<Chat> GetChatById(int chatId)
        {
            await GetBearerToken();
            return await _httpClient.GetFromJsonAsync<Chat>($"api/chat/{chatId}");
        }

        public async Task<IEnumerable<ChatInfoDTO>> GetAllChatsAsync()
        {
            await GetBearerToken();
            return await _httpClient.GetFromJsonAsync<IEnumerable<ChatInfoDTO>>("api/chat/all", _options);
        }
    }
}
