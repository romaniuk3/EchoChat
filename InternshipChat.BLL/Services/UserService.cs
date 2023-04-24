using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Entities;
using InternshipChat.DAL.Helpers;
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

        public PagedList<User> GetAll(UserParameters userParameters)
        {
            return _unitOfWork.UserRepository.GetUsers(userParameters);
        }

        public User GetUser(int id)
        {
            return _unitOfWork.UserRepository.GetById(u => u.Id == id);
        }

        public User Update(User user)
        {
            var updatedUser = _unitOfWork.UserRepository.Update(user);
            _unitOfWork.Save();
            return updatedUser;
        }
    }
}
