using InternshipChat.BLL.Helpers;
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
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<User> _userManager;

        public UserService(IUnitOfWork unitOfWork, IWebHostEnvironment environment, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _environment = environment;
            _userManager = userManager;
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

            return user!;
        }

        public async Task<User> UpdateAsync(int userId, UpdateUserDTO userDto)
        {
            //var user = await _userManager.FindByNameAsync(userDto.Email);
            var user = await _userManager.FindByIdAsync(userId.ToString());
            user.UserName = userDto.Email;
            user.Email = userDto.Email;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Avatar = userDto.Avatar;


            await _userManager.UpdateAsync(user);
            return user;
        }

        
        public async Task<string> SaveUserImageAsync(UpdateUserDTO userDto)
        {
            var uniqueFileName = FileHelper.GetUniqueFileName(userDto.AvatarImage.FileName);
            var uploads = Path.Combine(_environment.WebRootPath, "users", userDto.Email);
            var imagePath = Path.Combine(uploads, uniqueFileName);

            Directory.CreateDirectory(Path.GetDirectoryName(imagePath));
            await userDto.AvatarImage.CopyToAsync(new FileStream(imagePath, FileMode.Create));

            return imagePath;
        }
    }
}
