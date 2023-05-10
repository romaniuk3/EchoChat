using InternshipChat.DAL.Data;
using InternshipChat.DAL.Entities;
using InternshipChat.DAL.Helpers;
using InternshipChat.DAL.Repositories.Interfaces;
using InternshipChat.Shared.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace InternshipChat.DAL.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ChatContext chatContext) : base(chatContext)
        {}

        public async Task<PagedList<User>> GetUsersAsync(UserParameters userParameters)
        {
            var users = GetAll();
            users = SearchGlobal(users, userParameters.SearchTerm);
            users = ApplySort(users, userParameters.OrderBy);

            return await PagedList<User>.ToPagedListAsync(users, userParameters.PageNumber, userParameters.PageSize);
        }

        public async Task<User?> GetUserByNameAsync(string name)
        {
            return await GetAll().FirstOrDefaultAsync(u => u.NormalizedUserName == name.Normalize());
        }

        private IQueryable<User> SearchGlobal(IQueryable<User> users, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return users;

            return users.Where(u =>
                u.FirstName!.Contains(searchTerm) ||
                u.LastName!.Contains(searchTerm) ||
                u.Email.Contains(searchTerm));
        }

        private IQueryable<User> ApplySort(IQueryable<User> users, string orderByQueryString)
        {
            if (string.IsNullOrEmpty(orderByQueryString))
            {
                return users.OrderBy(u => u.Email);
            }

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(User).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach ( var param in orderParams) 
            {
                if (string.IsNullOrEmpty(param)) continue;

                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if(objectProperty == null) continue;

                var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                return users.OrderBy(u => u.Email);
            }

            return users.OrderBy(orderQuery);
        }
    }
}
