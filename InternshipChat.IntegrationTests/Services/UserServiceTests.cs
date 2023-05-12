using InternshipChat.BLL.Errors;
using InternshipChat.BLL.ServiceResult;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Data;
using InternshipChat.IntegrationTests.Helpers;
using InternshipChat.Shared.DTO.UserDtos;
using InternshipChat.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.IntegrationTests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private IUserService _userService;
        private ChatContext _chatContext;

        [OneTimeSetUp]
        public void SetUp()
        {
            _userService = ServicesHelper.GetUserService();
            _chatContext = ServicesHelper.GetDbContext();
        }

        [TestCase(2)]
        [TestCase(5)]
        public async Task GetAll_ByParameters_Returns_All(int pageSize)
        {
            await SeedDbHelper.ClearDb(_chatContext);

            var userParams = new UserParameters { PageSize = pageSize };
            var fakeUsers = await SeedDbHelper.SeedFakeUsers(_chatContext);

            var users = await _userService.GetAllAsync(userParams);

            Assert.That(users, Is.Not.Empty);
            Assert.That(users.Count, Is.EqualTo(pageSize));
        }

        [Test]
        public async Task Get_ById_Returns_User()
        {
            await SeedDbHelper.ClearDb(_chatContext);

            var fakeUsers = await SeedDbHelper.SeedFakeUsers(_chatContext);
            var randomUser = fakeUsers.FirstOrDefault();

            var userByIdResult = _userService.GetUser(randomUser.Id);

            Assert.That(userByIdResult.IsSuccess, Is.True);
            Assert.Multiple(() =>
            {
                Assert.That(userByIdResult.Value, Is.Not.Null);
                Assert.That(userByIdResult.Value.Id, Is.EqualTo(randomUser.Id));
            });
        }

        [Test]
        public async Task Get_ById_Returns_UserNotFoundError()
        {
            await SeedDbHelper.ClearDb(_chatContext);

            int nonExistingUserId = 999;
            var expectedError = Result.Failure(DomainErrors.User.NotFound).Error;

            var userByIdResult = _userService.GetUser(nonExistingUserId);

            Assert.That(userByIdResult.IsFailure, Is.True);
            Assert.That(userByIdResult.Error, Is.SameAs(expectedError));
        }

        [Test]
        public async Task UpdateUser_Returns_UpdatedUser()
        {
            await SeedDbHelper.ClearDb(_chatContext);

            var userToUpdate = await SeedDbHelper.AddUserToDb(_chatContext);
            var updateUserModel = new UpdateUserDTO
            {
                FirstName = "testupdatedname",
                LastName = "testupdatedlast",
                Email = "testupdated@gmail.com",
                Avatar = "data:avatar-updated"
            };

            var updateUserResult = await _userService.UpdateAsync(userToUpdate.Id, updateUserModel);
            var updatedUser = await _chatContext.Users.FirstOrDefaultAsync(u => u.Id == userToUpdate.Id);

            Assert.That(updateUserResult.IsSuccess);
            Assert.Multiple(() =>
            {
                Assert.That(updatedUser, Is.Not.Null);
                Assert.That(updatedUser.Id, Is.EqualTo(userToUpdate.Id));

                Assert.That(updatedUser.FirstName, Is.EqualTo(updateUserModel.FirstName));
                Assert.That(updatedUser.LastName, Is.EqualTo(updateUserModel.LastName));
                Assert.That(updatedUser.Email, Is.EqualTo(updateUserModel.Email));
                Assert.That(updatedUser.Avatar, Is.EqualTo(updateUserModel.Avatar));
                Assert.That(updatedUser.UserName, Is.EqualTo(updateUserModel.Email));
            });
        }

        [Test]
        public async Task UpdateUser_Returns_UserNotFoundError()
        {
            await SeedDbHelper.ClearDb(_chatContext);

            int nonExistingUserId = 999;
            var updateUserModel = new UpdateUserDTO();
            var expectedError = Result.Failure(DomainErrors.User.NotFound).Error;

            var updateUserResult = await _userService.UpdateAsync(nonExistingUserId, updateUserModel);
            
            Assert.That(updateUserResult.IsFailure, Is.True);
            Assert.That(updateUserResult.Error, Is.SameAs(expectedError));
        }
    }
}
