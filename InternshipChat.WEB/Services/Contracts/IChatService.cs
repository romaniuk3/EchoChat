using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO.ChatDtos;

namespace InternshipChat.WEB.Services.Contracts
{
    public interface IChatService
    {
        Task<HttpResponseMessage> CreateChat(CreateChatDTO chatDTO);
        Task<IEnumerable<ChatInfoDTO>> GetAllChatsAsync();
        Task<ChatDTO> GetChatById(int chatId);
        Task<HttpResponseMessage> AddUserToChat(int chatId, int userId);
    }
}
