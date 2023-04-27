using InternshipChat.DAL.Entities;
using InternshipChat.DAL.Helpers;
using InternshipChat.Shared.DTO;
using InternshipChat.Shared.DTO.UserDtos;
using InternshipChat.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.DAL.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<PagedList<User>> GetUsersAsync(UserParameters userParameters);
        Task<User?> GetUserByNameAsync(string name);
    }
}
