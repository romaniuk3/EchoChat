using InternshipChat.DAL.Data;
using InternshipChat.DAL.Entities;
using InternshipChat.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.DAL.Repositories
{
    public class UserChatsRepository : Repository<UserChats>, IUserChatsRepository
    {
        private readonly ChatContext _chatContext;

        public UserChatsRepository(ChatContext chatContext) : base(chatContext)
        {
            _chatContext = chatContext;
        }

        public async Task<IEnumerable<Chat>> GetAllUserChats(int userId)
        {
            var chats = await _chatContext.UserChats
                .Where(uc => uc.UserId == userId)
                .Include(uc => uc.Chat)
                .ThenInclude(c => c.UserChats)
                .ThenInclude(uc => uc.User)
                .Select(uc => uc.Chat)
                .ToListAsync();
            
            return chats;
        }
    }
}
