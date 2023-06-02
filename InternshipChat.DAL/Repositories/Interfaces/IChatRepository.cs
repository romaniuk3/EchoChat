using InternshipChat.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.DAL.Repositories.Interfaces
{
    public interface IChatRepository : IRepository<Chat>
    {
        Task<Chat> GetChatById(int id);
        Task<IEnumerable<ChatInfoView>> GetAllChats();
        Task<Chat?> GetChatByName(string name);
        Task<IEnumerable<ChatAttachment>> GetAllChatAttachments(int chatId);
    }
}
