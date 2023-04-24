using InternshipChat.DAL.Data;
using InternshipChat.DAL.Entities;
using InternshipChat.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ChatContext _chatContext;

        public Repository(ChatContext chatContext)
        {
            _chatContext = chatContext;
        }

        public void Add(T entity)
        {
            _chatContext.Set<T>().Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll()
        {
            return _chatContext.Set<T>().AsNoTracking();
        }

        public T Update(T entity)
        {
            return _chatContext.Set<T>().Update(entity).Entity;
        }

        public T GetById(Expression<Func<T, bool>> expression)
        {
            return _chatContext.Set<T>().FirstOrDefault(expression);
        }

        public void Remove(T entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }
    }
}
