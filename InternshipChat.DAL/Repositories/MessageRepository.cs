using InternshipChat.DAL.Data;
using InternshipChat.DAL.Entities;
using InternshipChat.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.DAL.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(ChatContext chatContext) : base(chatContext)
        {
        }
    }
}
