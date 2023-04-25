using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO.ChatDtos;

namespace InternshipChat.WEB.Services.Contracts
{
    public interface IChatService
    {
        Task CreateChat(ChatDTO chatDTO);
        Task<Chat> GetAllChats();
    }
}
