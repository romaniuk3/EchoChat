using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO.ChatDtos;

namespace InternshipChat.WEB.Services.Contracts
{
    public interface IMessageService
    {
        Task<List<Message>> GetMessages();
        Task SaveMessageAsync(MessageDTO message);
    }
}
