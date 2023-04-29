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
    public class ChatRepository : Repository<Chat>, IChatRepository
    {
        private readonly ChatContext _chatContext;

        public ChatRepository(ChatContext chatContext) : base(chatContext)
        {
            _chatContext = chatContext;
        }

        public async Task<Chat> GetChatById(int id)
        {
            var chat = await _chatContext.Chats
                .Include(x => x.UserChats)
                .ThenInclude(y => y.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            return chat;
        } 

        public async Task<Chat?> GetChatByName(string name)
        {
            return await _chatContext.Chats.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<IEnumerable<ChatInfoView>> GetAllChats()
        {
            var chats = await _chatContext.Set<ChatInfoView>().ToListAsync();
            return chats;
        }
    }
}
