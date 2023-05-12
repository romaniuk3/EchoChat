using AutoMapper;
using Bogus.DataSets;
using InternshipChat.BLL.Services;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.BLL.Validators;
using InternshipChat.DAL.Entities;
using InternshipChat.DAL.Repositories.Interfaces;
using InternshipChat.DAL.UnitOfWork;
using InternshipChat.Shared.DTO;
using InternshipChat.Shared.DTO.UserDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private Mock<LoginDtoValidator> _mockLoginValidator;
        private Mock<RegisterDTOValidator> _mockRegisterValidator;

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
            Assert.IsNotNull(loginResult.Token);
        }

        [Test]
        public async Task Login_Returns_InvalidDataError()
        {
            var loginModel = new LoginDto
            {
                Email = "test",
                Password = "test"
            };
            _mockUserManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(() => null);

            var loginResult = await _authService.Login(loginModel);

            Assert.IsFalse(loginResult.Successful);
        }

        [Test]
        public async Task ChangePassword_Returns_Success()
        {
            var user = new User();
            _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            _mockUserManager.Setup(um => um.ChangePasswordAsync(user, It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var changePasswordResult = await _authService.ChangePassword(new Shared.Models.ChangePasswordModel());

            Assert.IsTrue(changePasswordResult.IsSuccess);
        }

        [Test]
        public async Task ChangePassword_Returns_Failure()
        {
            _mockUserManager.Setup(um => um.ChangePasswordAsync(null, It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            var changePasswordResult = await _authService.ChangePassword(new Shared.Models.ChangePasswordModel());

            Assert.IsTrue(changePasswordResult.IsFailure);
        }

        [Test]
        public async Task Register_Returns_Success()
        {
            var user = InMemoryDatabase.GenerateFakeUser();
            var registerModel = new RegisterUserDTO
            {
                Email = user.Email
            };

            _mockMapper.Setup(x => x.Map<User>(It.IsAny<RegisterUserDTO>())).Returns(user);
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            var result = await _authService.Register(registerModel);

            Assert.IsTrue(result.IsSuccess);
        }

        [Test]
        public async Task Register_Returns_Failure()
        {
            var user = InMemoryDatabase.GenerateFakeUser();
            var registerModel = new RegisterUserDTO
            {
                Email = user.Email
            };

            _mockMapper.Setup(x => x.Map<User>(It.IsAny<RegisterUserDTO>())).Returns(user);
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());

            var result = await _authService.Register(registerModel);

            Assert.IsFalse(result.IsSuccess);
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
