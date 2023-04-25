using InternshipChat.DAL.Repositories;
using InternshipChat.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IMessageRepository MessageRepository { get; }
        IChatRepository ChatRepository { get; }
        IUserChatsRepository UserChatsRepository { get; }
        int Save();
    }
}
