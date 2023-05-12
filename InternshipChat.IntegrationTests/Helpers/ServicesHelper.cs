using AutoMapper;
using FluentValidation;
using InternshipChat.BLL.Mapping;
using InternshipChat.BLL.Services;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.BLL.Validators;
using InternshipChat.DAL.Data;
using InternshipChat.DAL.Entities;
using InternshipChat.DAL.Repositories;
using InternshipChat.DAL.Repositories.Interfaces;
using InternshipChat.DAL.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.IntegrationTests.Helpers
{
    public class ServicesHelper
    {
        private static readonly string LOCALDB_CONNECTION_STRING = "Data Source=(localdb)\\LocalDB;Initial Catalog=InternshipChatLocal;Integrated Security=true;";
        private static ChatContext _chatContext;

        public static IChatService GetChatService()
        {
            CreateDbContext();
            var serviceProvider = SetupServiceProviders();

            var unitOfWork = new UnitOfWork(_chatContext, serviceProvider);
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile())));
            var chatService = new ChatService(unitOfWork, mapper);

            return chatService;
        }

        public static IUserService GetUserService()
        {
            CreateDbContext();
            var serviceProvider = SetupServiceProviders();

            var unitOfWork = new UnitOfWork(_chatContext, serviceProvider);
            var userManager = serviceProvider.GetService<UserManager<User>>();
            var userService = new UserService(unitOfWork, userManager);

            return userService;
        }

        public static IMessageService GetMessageService()
        {
            CreateDbContext();
            var serviceProvider = SetupServiceProviders();

            var unitOfWork = new UnitOfWork(_chatContext, serviceProvider);
            var messageService = new MessageService(unitOfWork);

            return messageService;
        }

        public static IAuthService GetAuthService()
        {
            CreateDbContext();
            var serviceProvider = SetupServiceProviders();

            var userManager = serviceProvider.GetService<UserManager<User>>();
            var loginValidator = serviceProvider.GetService<LoginDtoValidator>();
            var registerValidator = serviceProvider.GetService<RegisterDTOValidator>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile())));
            var configuration = InitConfiguration();
            var authService = new AuthService(mapper, userManager, configuration, loginValidator, registerValidator);

            return authService;
        }

        public static ChatContext GetDbContext()
        {
            return _chatContext;
        }

        private static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings-test.json")
                .AddEnvironmentVariables()
                .Build();

            return config;
        }

        private static void CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ChatContext>()
                .UseSqlServer(LOCALDB_CONNECTION_STRING);

            _chatContext = new ChatContext(optionsBuilder.Options);
            _chatContext.Database.EnsureCreated();
        }

        private static ServiceProvider SetupServiceProviders()
        {
            var services = new ServiceCollection();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IUserChatsRepository, UserChatsRepository>();
            services.AddScoped(_ => _chatContext);
            services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UserDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<RegisterDTOValidator>();
            services.AddIdentityCore<User>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ChatContext>();

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
