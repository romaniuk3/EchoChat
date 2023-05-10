using AutoMapper;
using InternshipChat.BLL.Services;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Entities;
using InternshipChat.DAL.Repositories.Interfaces;
using InternshipChat.DAL.UnitOfWork;
using InternshipChat.Shared.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.UnitTests.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private IConfiguration _configuration;
        private IAuthService _authService;
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<IMapper> _mockMapper;
        private Mock<IConfiguration> _mockConfiguration;

        [SetUp]
        public void SetUp() 
        {
            _configuration = InitConfiguration();
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
            _mockMapper = new Mock<IMapper>();
            _mockConfiguration = new Mock<IConfiguration>();
            _authService = new AuthService(_mockMapper.Object, _mockUserManager.Object, _configuration);
        }

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings-test.json")
                .AddEnvironmentVariables()
                .Build();

            return config;
        }

        [Test]
        public async Task Login_Returns_SuccessToken()
        {
            var loginModel = new LoginDto
            {
                Email = "test",
                Password = "test"
            };

            _mockUserManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(InMemoryDatabase.GenerateFakeUser());
            _mockUserManager.Setup(um => um.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);
            SetUpTokenConfig();

            var loginResult = await _authService.Login(loginModel);

            Assert.IsTrue(loginResult.Successful);
        }

        public void SetUpTokenConfig()
        {
            _mockConfiguration.Setup(c => c["JwtSettings:Key"]).Returns(_configuration["JwtSettings:Key"]);
            _mockConfiguration.Setup(c => c["JwtSettings:Issuer"]).Returns(_configuration["JwtSettings:Issuer"]);
            _mockConfiguration.Setup(c => c["JwtSettings:Audience"]).Returns(_configuration["JwtSettings:Audience"]);
            _mockConfiguration.Setup(c => c["JwtSettings:DurationInMinutes"]).Returns(_configuration["JwtSettings:DurationInMinutes"]);
        }
    }
}
