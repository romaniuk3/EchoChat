using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Entities;
using InternshipChat.DAL.Helpers;
using InternshipChat.DAL.Repositories.Interfaces;
using InternshipChat.DAL.UnitOfWork;
using InternshipChat.Shared.DTO;
using InternshipChat.Shared.DTO.UserDtos;
using InternshipChat.Shared.Models;
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

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedList<User>> GetAllAsync(UserParameters userParameters)
        {
            var repository = _unitOfWork.GetRepository<IUserRepository>();

            return await repository.GetUsersAsync(userParameters);
        }

        public User GetUser(int id)
        {
            var repository = _unitOfWork.GetRepository<IUserRepository>();
            return repository.GetById(u => u.Id == id);
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            var repository = _unitOfWork.GetRepository<IUserRepository>();
            var user = await repository.GetUserByNameAsync(name);

            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            var repository = _unitOfWork.GetRepository<IUserRepository>();
            var updatedUser = repository.Update(user);
            _unitOfWork.Save();
            return updatedUser;
        }
    }
}
