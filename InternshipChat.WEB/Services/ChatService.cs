using Blazored.LocalStorage;
using InternshipChat.Shared.DTO.ChatDtos;
using InternshipChat.WEB.Services.Base;
using InternshipChat.WEB.Services.Contracts;
using Microsoft.AspNetCore.WebUtilities;
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

        public async Task<HttpResponseMessage> CreateChat(CreateChatDTO createChatDTO)
        {
            await GetBearerToken();
            return await _httpClient.PostAsJsonAsync("api/chat/create", createChatDTO, _options);
        }

        public async Task<HttpResponseMessage> AddUserToChat(int chatId, int userId)
        {
            await GetBearerToken();
            return await _httpClient.PostAsJsonAsync($"api/chat/{chatId}/add-user", userId, _options);
        }

        public async Task<ChatDTO> GetChatById(int chatId)
        {
            await GetBearerToken();
            return await _httpClient.GetFromJsonAsync<ChatDTO>($"api/chat/{chatId}");
        }

        public async Task<IEnumerable<ChatInfoDTO>> GetAllChatsAsync()
        {
            await GetBearerToken();
            return await _httpClient.GetFromJsonAsync<IEnumerable<ChatInfoDTO>>("api/chat/all", _options);
        }

        public async Task<List<ChatAttachmentDTO>> GetChatAttachments(int chatId)
        {
            await GetBearerToken();
            return await _httpClient.GetFromJsonAsync<List<ChatAttachmentDTO>>($"api/chat/attachments/{chatId}");
        }

        public async Task<List<ChatAttachmentDTO>> GetUserSignatureAttachments(int chatId, int userId)
        {
            await GetBearerToken();
            var queryParameters = new Dictionary<string, string>
                {
                    { "userId", userId.ToString() }
                };

            var urlWithQuery = QueryHelpers.AddQueryString($"api/chat/signature-attachments/{chatId}", queryParameters);

            return await _httpClient.GetFromJsonAsync<List<ChatAttachmentDTO>>(urlWithQuery);
        }
    }
}
