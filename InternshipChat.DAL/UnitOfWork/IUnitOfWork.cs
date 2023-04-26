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
        T GetRepository<T>();
        int Save();
    }
}
