using Bogus;
using InternshipChat.DAL.Data;
using InternshipChat.DAL.Entities;
using InternshipChat.DAL.Repositories;
using InternshipChat.DAL.Repositories.Interfaces;
using InternshipChat.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.UnitTests.Repositories
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private IUserRepository _userRepository;
        private ChatContext _chatContext;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            _chatContext = await InMemoryDatabase.GetDbContext(); 
            _userRepository = new UserRepository(_chatContext);
        }
        
        [Test]
        public void Create_Should_AddUser_To_Db()
        {
            var user = InMemoryDatabase.GenerateFakeUser();

            _userRepository.Add(user);
            _chatContext.SaveChanges();

            var addedUser = _userRepository.GetById(u => u.Id == user.Id);

            Assert.IsNotNull(addedUser);
            Assert.AreEqual(user.Id, addedUser.Id);
        }

        [Test]
        public async Task GetAll_WithoutParameters_Returns_AllUsersByPageSize()
        {
            var userParams = new UserParameters();

            var allUsersByParameters = await _userRepository.GetUsersAsync(userParams);

            Assert.AreEqual(userParams.PageSize, allUsersByParameters.Count);
        }

        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(4, 4)]
        [TestCase(10, 10)]
        public async Task GetAll_WithPageParameters_Returns_AllUserByPageSize(int pageSize, int expectedUsersCount)
        {
            var userParams = new UserParameters
            {
                PageSize = pageSize,
            };

            var allUsersByParameters = await _userRepository.GetUsersAsync(userParams);

            Assert.AreEqual(expectedUsersCount, allUsersByParameters.Count);
        }

        [Test]
        public void GetUserBy_ExistingId_Returns_User()
        {
            var expectedUser = _chatContext.Users.FirstOrDefault(u => u.Id == 2);
            var user = _userRepository.GetById(u => u.Id == 2);

            Assert.IsNotNull(user);
            Assert.AreEqual(expectedUser.UserName, user.UserName);
        }

        [Test]
        public void GetUserBy_NonExistingId_Returns_Null()
        {
            var user = _userRepository.GetById(u => u.Id == 9999);

            Assert.IsNull(user);
        }
    }
}
