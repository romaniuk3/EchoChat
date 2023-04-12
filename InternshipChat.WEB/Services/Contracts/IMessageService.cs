using InternshipChat.DAL.Entities;

namespace InternshipChat.WEB.Services.Contracts
{
    public interface IMessageService
    {
        Task<List<Message>> GetMessages();
    }
}
