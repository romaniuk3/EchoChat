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
using System.ComponentModel.DataAnnotations;
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
                ConfirmPassword = "P@ssword1",
                Birthdate = DateTime.Parse("01-01-1991")
            };

            var userBeforeCreating = await _chatContext.Users.FirstOrDefaultAsync();
            var createUserResult = await _authService.Register(registerModel);
            var userAfterCreating = await _chatContext.Users.FirstOrDefaultAsync();

            Assert.That(userBeforeCreating, Is.Null);
            Assert.That(createUserResult.IsSuccess, Is.True);
            Assert.Multiple(() =>
            {
                Assert.That(userAfterCreating, Is.Not.Null);
                Assert.That(userAfterCreating.FirstName, Is.EqualTo(registerModel.FirstName));
                Assert.That(userAfterCreating.Email, Is.EqualTo(registerModel.Email));
            });
        }

        [Test]
        [TestCaseSource(nameof(RegistrationValidationTestCases))]
        public async Task Register_Returns_ValidationErrors(string firstName, string lastName, string email, 
            string password, string confirmPassword, string birthDate, string expectedError)
        {
            var registerModel = new RegisterUserDTO
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword,
                Birthdate = DateTime.Parse(birthDate)
            };

            var createUserResult = await _authService.Register(registerModel);

            Assert.That(createUserResult.IsFailure, Is.True);
            Assert.That(createUserResult.Error.Messages.First(), Is.EqualTo(expectedError));
        }

        public static IEnumerable<TestCaseData> RegistrationValidationTestCases()
        {
            yield return new TestCaseData("John", "Doe", "email@example.com", "P@ss1", "P@ss1", "1990-01-01", 
                "Password must be at least 8 characters.");

            yield return new TestCaseData("John", "Doe", "email@example.com", "p@ssword1", "p@ssword1", "1990-01-01",
                "Password must contain at least one uppercase letter.");

            yield return new TestCaseData("John", "Doe", "email@example.com", "P@SSWORD1", "P@SSWORD1", "1990-01-01",
                "Password must contain at least one lowercase letter.");

            yield return new TestCaseData("John", "Doe", "email@example.com", "P@ssword", "P@ssword", "1990-01-01",
                "Password must contain at least one digit.");

            yield return new TestCaseData("John", "Doe", "email@example.com", "Password1", "Password1", "1990-01-01",
                "Password must contain at least one special character.");

            yield return new TestCaseData("John", "Doe", "email@example.com", "P@ssword1", "P@ssword5", "1990-01-01",
                "Password must match.");

            yield return new TestCaseData("Joh12n1", "Doe", "email@example.com", "Password1", "Password1", "1990-01-01",
                "First Name must contain only letters");

            yield return new TestCaseData("John", "D2oe", "email@example.com", "Password1", "Password1", "1990-01-01",
                "Last Name must contain only letters");
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
                Assert.That(loginResult.IsSuccess, Is.True);
                Assert.That(loginResult.Value.Token, Is.Not.Empty);
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
            var expectedError = Result.Failure(DomainErrors.Auth.IncorrectData).Error;

            var loginResult = await _authService.Login(loginModel);

            Assert.That(registeredUser, Is.Not.Null);
            Assert.That(loginResult.IsFailure, Is.True);
            Assert.That(loginResult.Error, Is.SameAs(expectedError));
        }

        [TestCase("test@email.com", "P@ssw1", "Password must be at least 8 characters.")]
        [TestCase("test@email.com", "password@1", "Password must contain at least one uppercase letter.")]
        [TestCase("test@email.com", "P@SSWORD1", "Password must contain at least one lowercase letter.")]
        [TestCase("test@email.com", "P@ssword", "Password must contain at least one digit.")]
        [TestCase("test@email.com", "Password1", "Password must contain at least one special character.")]
        [TestCase("testmail", "Password1", "'Email' is not a valid email address.")]
        public async Task Login_Returns_ValidationErrors(string email, string password, string expectedError)
        {
            var loginModel = new LoginDto
            {
                Email = email,
                Password = password
            };

            var loginResult = await _authService.Login(loginModel);

            Assert.That(loginResult.IsFailure, Is.True);
            Assert.That(loginResult.Error.Messages.First(), Is.EqualTo(expectedError));
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
