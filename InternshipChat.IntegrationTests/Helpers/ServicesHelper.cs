using AutoMapper;
using InternshipChat.BLL.Mapping;
using InternshipChat.BLL.Services;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Data;
using InternshipChat.DAL.Repositories;
using InternshipChat.DAL.Repositories.Interfaces;
using InternshipChat.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
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

        public static ChatContext GetDbContext()
        {
            return _chatContext;
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

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
