using InternshipChat.BLL.Errors;
using InternshipChat.BLL.ServiceResult;
using InternshipChat.BLL.Services;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Entities;
using InternshipChat.DAL.Helpers;
using InternshipChat.DAL.Repositories.Interfaces;
using InternshipChat.DAL.UnitOfWork;
using InternshipChat.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.UnitTests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private IUserService _userService;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<IFileService> _mockIFileService;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUserManager = new Mock<UserManager<User>>(
                        Mock.Of<IUserStore<User>>(),
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null
                    );
            _mockIFileService = new Mock<IFileService>();
            _mockUnitOfWork.Setup(uow => uow.GetRepository<IUserRepository>()).Returns(_mockUserRepository.Object);
            _userService = new UserService(_mockUnitOfWork.Object, _mockUserManager.Object, _mockIFileService.Object);
        }

        [Test]
        public void GetUser_ById_Returns_User()
        {
            var id = 2; 
            var expectedUserValue = new User
            {
                Id = id,
                FirstName = "Dan",
                UserName = "Ben",
            };
            var expectedResult = Result.Success(expectedUserValue);

            _mockUserRepository.Setup(r => r.GetById(u => u.Id == id)).Returns(expectedUserValue);

            var result = _userService.GetUser(2);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreSame(expectedUserValue, result.Value);
        }

        [Test]
        public void GetUser_ById_ReturnsError_NotFound()
        {
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<Expression<Func<User, bool>>>())).Returns(() => null);
            var expectedError = Result.Failure<User>(DomainErrors.User.NotFound);

            var userResult = _userService.GetUser(2);

            Assert.IsTrue(userResult.IsFailure);
            Assert.AreEqual(expectedError.Error, userResult.Error);
        }

        [Test]
        public async Task GetAll_ByParameters_Returns_All()
        {
            var userParams = new UserParameters()
            {
                PageSize = 2
            };

            var users = new List<User>
            {
                InMemoryDatabase.GenerateFakeUser(),
                InMemoryDatabase.GenerateFakeUser(),
                InMemoryDatabase.GenerateFakeUser(),
            };
            var expectedUsersList = PagedList<User>.ToPagedList(users, userParams.PageNumber, userParams.PageSize);
            
            _mockUserRepository.Setup(repo => repo.GetUsersAsync(userParams)).ReturnsAsync(expectedUsersList);

            var usersList = await _userService.GetAllAsync(userParams);

            Assert.IsNotEmpty(usersList);
            Assert.AreEqual(userParams.PageSize, usersList.Count);
        }
    }
}
