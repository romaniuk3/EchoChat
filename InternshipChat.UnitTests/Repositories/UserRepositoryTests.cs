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
        private string UNIQUE_USERNAME = "UniqueUserName@3";

        private static DbContextOptions<ChatContext> dbContextOptions = new DbContextOptionsBuilder<ChatContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        [OneTimeSetUp]
        public void SetUp()
        {
            _chatContext = new ChatContext(dbContextOptions);
            _chatContext.Database.EnsureCreated();
            _userRepository = new UserRepository(_chatContext);
            SeedDatabase();
        }
        
        [Test]
        public void Create_Should_AddUser_To_Db()
        {
            var user = GenerateFakeUser();

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
            var allUsers = _userRepository.GetAll().ToList();

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

        [Test]
        public async Task GetAll_FilteredByName_Returns_FilteredUsersInPagedList()
        {
            var userParams = new UserParameters
            {
                SearchTerm = UNIQUE_USERNAME
            };

            int expectedCount = 2;

            var allUsersByParameters = await _userRepository.GetUsersAsync(userParams);
            
            Assert.AreEqual(expectedCount, allUsersByParameters.Count);
        }

        private User GenerateFakeUser()
        {
            var faker = new Faker<User>()
                .RuleFor(u => u.Id, f => f.IndexGlobal + 1)
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.UserName, f => f.Internet.Email())
                .RuleFor(u => u.Email, (f, u) => u.UserName)
                .RuleFor(u => u.NormalizedEmail, (f, u) => u.Email.ToUpper())
                .RuleFor(u => u.NormalizedUserName, (f, u) => u.UserName.ToUpper());

            return faker;
        }

        private User UserWithCustomNameForFilterTest(User user, int index)
        {
            string newUserName = UNIQUE_USERNAME + index;
            user.FirstName = newUserName;       
            user.LastName = newUserName;
            
            return user;
        }

        private void SeedDatabase()
        {
            var users = new List<User>();
            for (int i = 0; i < 16; i++)
            {
                var user = GenerateFakeUser();
                if (i >= 14)
                {
                    users.Add(UserWithCustomNameForFilterTest(user, i));
                } else
                {
                    users.Add(user);
                }
            }

            _chatContext.AddRange(users);
            _chatContext.SaveChanges();
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            _chatContext.Database.EnsureDeleted();
        }
    }
}
