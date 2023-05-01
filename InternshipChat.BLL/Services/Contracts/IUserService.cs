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

namespace InternshipChat.BLL.Services.Contracts
{
    public interface IUserService
    {
        public Task<PagedList<User>> GetAllAsync(UserParameters userParameters);
        public User GetUser(int id);
        public Task<User> UpdateAsync(int id, UpdateUserDTO user);
        public Task<User> GetUserByNameAsync(string name);
        Task<string> SaveUserImageAsync(UpdateUserDTO userDto);
    }
}
