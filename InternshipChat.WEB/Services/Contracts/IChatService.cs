using InternshipChat.Shared.DTO.ChatDtos;

namespace InternshipChat.WEB.Services.Contracts
{
    public interface IChatService
    {
        Task<HttpResponseMessage> CreateChat(CreateChatDTO chatDTO);
        Task<IEnumerable<ChatInfoDTO>> GetAllChatsAsync();
        Task<ChatDTO> GetChatById(int chatId);
        Task<HttpResponseMessage> AddUserToChat(int chatId, int userId);
        Task<List<ChatAttachmentDTO>> GetChatAttachments(int chatId);
        Task<List<ChatAttachmentDTO>> GetUserSignatureAttachments(int chatId, int userId);
    }
}
