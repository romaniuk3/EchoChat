using InternshipChat.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.BLL.Services.Contracts
{
    public interface IMessageService
    {
        public Message SendMessage(Message message);
        public Task<IEnumerable<Message>> GetMessagesAsync(int chatId);
    }
}
