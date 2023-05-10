using InternshipChat.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.DAL.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);
        T? GetById(Expression<Func<T, bool>> expression);
        IQueryable<T> GetAll();
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        T Update(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
