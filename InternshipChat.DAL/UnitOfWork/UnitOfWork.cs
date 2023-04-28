using InternshipChat.DAL.Data;
using InternshipChat.DAL.Repositories;
using InternshipChat.DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ChatContext _chatContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, object> _repositories = new();

        public UnitOfWork(ChatContext chatContext, IServiceProvider serviceProvider)
        {
            _chatContext = chatContext;
            _serviceProvider = serviceProvider;
        }

        public T GetRepository<T>()
        {
            var typeName = typeof(T).Name;
            if (!_repositories.ContainsKey(typeName))
            {
                T repository = _serviceProvider.GetService<T>() ??
                    throw new ArgumentNullException($"Repository {typeName} was not found");

                _repositories.Add(typeName, repository);
            }

            return (T)_repositories[typeName];
        }

        public int Save()
        {
            return _chatContext.SaveChanges();
        }

        public void Dispose()
        {
            //Dispose(true);
        }

        /*
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _chatContext.Dispose();
                }
            }
            _isDisposed = true;
        }
        */
    }
}
