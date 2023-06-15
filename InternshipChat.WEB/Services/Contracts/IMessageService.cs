using InternshipChat.Shared.DTO.ChatDtos;

namespace InternshipChat.WEB.Services.Contracts
{
    public interface IMessageService
    {
        Task<List<MessageDTO>> GetMessagesAsync(int chatId);
        Task SaveMessageAsync(MessageDTO message);
    }
}
