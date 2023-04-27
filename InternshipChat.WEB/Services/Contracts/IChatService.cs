using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO.ChatDtos;

namespace InternshipChat.WEB.Services.Contracts
{
    public interface IChatService
    {
        Task CreateChat(CreateChatDTO chatDTO);
        Task<IEnumerable<Chat>> GetAllChats();
        Task<Chat> GetChatById(int chatId);
    }
}
