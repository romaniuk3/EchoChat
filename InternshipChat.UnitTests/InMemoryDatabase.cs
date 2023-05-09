using Bogus;
using InternshipChat.DAL.Data;
using InternshipChat.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.UnitTests
{
    public class InMemoryDatabase
    {
        private static ChatContext _chatContext;
        private static DbContextOptions<ChatContext> dbContextOptions = new DbContextOptionsBuilder<ChatContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        private static string UNIQUE_USERNAME = "UniqueUserName@3";

        public static async Task<ChatContext> GetDbContext()
        {
            if (_chatContext == null)
            {
                await SetDbContext();
            }

            return _chatContext;
        }

        public static async Task SetDbContext()
        {
            _chatContext = new ChatContext(dbContextOptions);
            _chatContext.Database.EnsureCreated();
            await SeedDatabase();
        }

        public static User GenerateFakeUser()
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

        public static Chat GenerateFakeChat()
        {
            var faker = new Faker<Chat>()
                .RuleFor(u => u.Id, f => f.IndexGlobal + 1)
                .RuleFor(u => u.Name, f => f.Lorem.Word())
                .RuleFor(u => u.Description, f => f.Lorem.Word());

            return faker;
        }

        private static User UserWithCustomNameForFilterTest(User user, int index)
        {
            string newUserName = UNIQUE_USERNAME + index;
            user.FirstName = newUserName;
            user.LastName = newUserName;

            return user;
        }

        private static async Task SeedDatabase()
        {
            var users = new List<User>();
            for (int i = 0; i < 16; i++)
            {
                var user = GenerateFakeUser();
                if (i >= 14)
                {
                    users.Add(UserWithCustomNameForFilterTest(user, i));
                }
                else
                {
                    users.Add(user);
                }
            }

            var chats = new List<Chat>();
            var userChats = new List<UserChats>();

            for (int i = 0; i < 5; i++)
            {
                var chat = GenerateFakeChat();
                chats.Add(chat);

                foreach(var user in users.Take(5))
                {
                    var userChat = new UserChats
                    {
                        Chat = chat,
                        User = user
                    };

                    userChats.Add(userChat);
                }
            }

            await _chatContext.Users.AddRangeAsync(users);
            await _chatContext.Chats.AddRangeAsync(chats);
            await _chatContext.UserChats.AddRangeAsync(userChats);
            await _chatContext.SaveChangesAsync();
        }
    }
}
