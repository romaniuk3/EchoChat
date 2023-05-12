using InternshipChat.BLL.ServiceResult;
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
        public Result<User> GetUser(int id);
        public Task<Result<User>> UpdateAsync(int id, UpdateUserDTO user);
    }
}
