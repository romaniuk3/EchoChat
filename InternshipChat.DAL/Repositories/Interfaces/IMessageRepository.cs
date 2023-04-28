using InternshipChat.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.DAL.Repositories.Interfaces
{
    public interface IMessageRepository : IRepository<Message>
    {
        public Task<IEnumerable<Message>> GetMessages(int chatId);
    }
}
