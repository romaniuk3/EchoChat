using InternshipChat.DAL.Data;
using InternshipChat.DAL.Repositories;
using InternshipChat.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ChatContext _chatContext;
        public IUserRepository UserRepository { get; private set; }

        public IMessageRepository MessageRepository { get; private set; }

        public UnitOfWork(ChatContext chatContext)
        {
            _chatContext = chatContext;
            
            UserRepository = new UserRepository(chatContext);
            MessageRepository = new MessageRepository(chatContext);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            return _chatContext.SaveChanges();
        }
    }
}
