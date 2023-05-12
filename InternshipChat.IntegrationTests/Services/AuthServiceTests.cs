using InternshipChat.BLL.Errors;
using InternshipChat.BLL.ServiceResult;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Data;
using InternshipChat.IntegrationTests.Helpers;
using InternshipChat.Shared.DTO;
using InternshipChat.Shared.DTO.UserDtos;
using InternshipChat.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.IntegrationTests.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private IAuthService _authService;
        private ChatContext _chatContext;

        [OneTimeSetUp]
        public void SetUp()
        {
            _authService = ServicesHelper.GetAuthService();
            _chatContext = ServicesHelper.GetDbContext();
        }

        [Test]
        [Order(1)]
        public async Task Register_Returns_Success()
        {
            await SeedDbHelper.ClearDb(_chatContext);

            var registerModel = new RegisterUserDTO
            {
                FirstName = "testname",
                LastName = "testlast",
                Email = "test@mail.com",
                Password = "P@ssword1",
                ConfirmPassword = "P@ssword1"
            };

            var userBeforeCreating = await _chatContext.Users.FirstOrDefaultAsync();
            var createUserResult = await _authService.Register(registerModel);
            var userAfterCreating = await _chatContext.Users.FirstOrDefaultAsync();

            Assert.That(userBeforeCreating, Is.Null);
            Assert.That(createUserResult.Succeeded, Is.True);
            Assert.Multiple(() =>
            {
                Assert.That(userAfterCreating, Is.Not.Null);
                Assert.That(userAfterCreating.FirstName, Is.EqualTo(registerModel.FirstName));
                Assert.That(userAfterCreating.Email, Is.EqualTo(registerModel.Email));
            });
        }

        [Test]
        [Order(2)]
        public async Task Login_Returns_SuccessToken()
        {
            var registeredUser = await _chatContext.Users.FirstOrDefaultAsync();
            var loginModel = new LoginDto
            {
                Email = registeredUser.Email,
                Password = "P@ssword1"
            };

            var loginResult = await _authService.Login(loginModel);

            Assert.That(registeredUser, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(loginResult.Successful, Is.True);
                Assert.That(loginResult.Token, Is.Not.Empty);
            });
        }

        [Test]
        [Order(3)]
        public async Task ChangePassword_Returns_Success()
        {
            var registeredUser = await _chatContext.Users.FirstOrDefaultAsync();
            var changePasswordModel = new ChangePasswordModel
            {
                Id = registeredUser.Id,
                CurrentPassword = "P@ssword1",
                NewPassword = "P@ssword2",
                ConfirmNewPassword = "P@ssword2"
            };

            var changePasswordResult = await _authService.ChangePassword(changePasswordModel);

            Assert.That(registeredUser, Is.Not.Null);
            Assert.That(changePasswordResult.IsSuccess, Is.True);
        }

        [Test]
        [Order(4)]
        public async Task Login_Returns_IncorrectPasswordError()
        {
            var registeredUser = await _chatContext.Users.FirstOrDefaultAsync();
            var loginModel = new LoginDto
            {
                Email = registeredUser.Email,
                Password = "P@ssword1"
            };

            var loginResult = await _authService.Login(loginModel);

            Assert.That(registeredUser, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(loginResult.Successful, Is.False);
                Assert.That(loginResult.Token, Is.Null);
            });
        }

        [Test]
        public async Task ChangePassword_Returns_UserNotFoundError()
        {
            var changePasswordModel = new ChangePasswordModel
            {
                Id = 0,
                CurrentPassword = "P@ssword1",
                NewPassword = "P@ssword2",
                ConfirmNewPassword = "P@ssword2"
            };
            var expectedError = Result.Failure(DomainErrors.User.NotFound).Error;

            var changePasswordResult = await _authService.ChangePassword(changePasswordModel);

            Assert.That(changePasswordResult.IsFailure, Is.True);
            Assert.That(changePasswordResult.Error, Is.SameAs(expectedError));
        }

        [Test]
        public async Task ChangePassword_Returns_IncorrectPasswordError()
        {
            await SeedDbHelper.ClearDb(_chatContext);

            var user = await SeedDbHelper.AddUserToDb(_chatContext);

            var changePasswordModel = new ChangePasswordModel
            {
                Id = user.Id,
                CurrentPassword = "P@ssword1",
                NewPassword = "P@ssword2",
                ConfirmNewPassword = "P@ssword2"
            };
            var expectedError = Result.Failure(DomainErrors.User.IncorrectPassword).Error;

            var changePasswordResult = await _authService.ChangePassword(changePasswordModel);

            Assert.That(changePasswordResult.IsFailure, Is.True);
            Assert.That(changePasswordResult.Error, Is.SameAs(expectedError));
        }
    }
}
