using InternshipChat.BLL.Errors;
using InternshipChat.BLL.ServiceResult;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Entities;
using InternshipChat.DAL.Helpers;
using InternshipChat.DAL.Repositories.Interfaces;
using InternshipChat.DAL.UnitOfWork;
using InternshipChat.Shared.DTO;
using InternshipChat.Shared.DTO.UserDtos;
using InternshipChat.Shared.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public UserService(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<PagedList<User>> GetAllAsync(UserParameters userParameters)
        {
            var repository = _unitOfWork.GetRepository<IUserRepository>();

            return await repository.GetUsersAsync(userParameters);
        }

        public Result<User> GetUser(int id)
        {
            var repository = _unitOfWork.GetRepository<IUserRepository>();
            var user = repository.GetById(u => u.Id == id);

            if (user == null)
            {
                return Result.Failure<User>(DomainErrors.User.NotFound);
            }

            return user;
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            var repository = _unitOfWork.GetRepository<IUserRepository>();
            var user = await repository.GetUserByNameAsync(name);

            return user!;
        }

        public async Task<User> UpdateAsync(int userId, UpdateUserDTO userDto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            user.UserName = userDto.Email;
            user.Email = userDto.Email;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Avatar = userDto.Avatar;


            await _userManager.UpdateAsync(user);
            return user;
        }
    }
}
