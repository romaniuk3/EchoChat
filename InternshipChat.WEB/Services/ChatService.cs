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

        public async Task CreateChat(ChatDTO chatDTO)
        {
            await GetBearerToken();
            await _httpClient.PostAsJsonAsync("api/chats/create", chatDTO, _options);
        }

        public async Task<Chat> GetAllChats()
        {
            await GetBearerToken();
            return await _httpClient.GetFromJsonAsync<Chat>("api/chats/all", _options);
        }
    }
}
