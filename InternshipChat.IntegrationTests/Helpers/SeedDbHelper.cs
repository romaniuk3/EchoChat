using Bogus;
using InternshipChat.DAL.Data;
using InternshipChat.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.IntegrationTests.Helpers
{
    public class SeedDbHelper
    {
        private static readonly int COUNT_OF_GENERATED_ENTITIES = 5;

        public static async Task<List<User>> SeedFakeUsers(ChatContext chatContext)
        {
            var users = new List<User>();
            for (int i = 0; i < COUNT_OF_GENERATED_ENTITIES; i++)
            {
                users.Add(UserDataHelper.GenerateFakeUser());
            }

            await chatContext.Users.AddRangeAsync(users);
            await chatContext.SaveChangesAsync();

            return await chatContext.Users.ToListAsync();
        }

        public static async Task<User?> AddUserToDb(ChatContext chatContext)
        {
            var user = UserDataHelper.GenerateFakeUser();

            await chatContext.Users.AddAsync(user);
            await chatContext.SaveChangesAsync();

            return await chatContext.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);
        }

        public static async Task<Chat?> AddChatToDb(ChatContext chatContext)
        {
            var chat = ChatDataHelper.GenerateFakeChat();

            await chatContext.Chats.AddAsync(chat);
            await chatContext.SaveChangesAsync();

            return await chatContext.Chats.FirstOrDefaultAsync(c => c.Name == chat.Name);
        }

        public static async Task<Chat?> AddChatToDatabase(ChatContext chatContext, Chat fakeChat)
        {
            await chatContext.Chats.AddAsync(fakeChat);
            await chatContext.SaveChangesAsync();

            return await chatContext.Chats.FirstOrDefaultAsync(c => c.Name == fakeChat.Name);
        } 

        public static async Task<List<Chat>> SeedFakeChats(ChatContext chatContext)
        {
            var chats = new List<Chat>();
            for (int i = 0; i < COUNT_OF_GENERATED_ENTITIES; i++)
            {
                chats.Add(ChatDataHelper.GenerateFakeChat());
            }

            await chatContext.Chats.AddRangeAsync(chats);
            await chatContext.SaveChangesAsync();

            return await chatContext.Chats.ToListAsync();
        }

        public static async Task<List<UserChats>> SeedFakeUserChats(ChatContext chatContext)
        {
            var users = await SeedFakeUsers(chatContext);
            var chats = await SeedFakeChats(chatContext);

            var userChats = new List<UserChats>();

            for (int i = 0; i < COUNT_OF_GENERATED_ENTITIES; i++)
            {
                var userChat = new UserChats
                {
                    Chat = chats[i],
                    User = users[i]
                };
                userChats.Add(userChat);
            }

            await chatContext.UserChats.AddRangeAsync(userChats);
            await chatContext.SaveChangesAsync();

            return userChats;
        }
        
        public static async Task ClearDb(ChatContext chatContext)
        {
            await chatContext.Database.ExecuteSqlRawAsync("DELETE FROM [AspNetRoleClaims]");
            await chatContext.Database.ExecuteSqlRawAsync("DELETE FROM [AspNetRoles]");
            await chatContext.Database.ExecuteSqlRawAsync("DELETE FROM [AspNetUserClaims]");
            await chatContext.Database.ExecuteSqlRawAsync("DELETE FROM [AspNetUserLogins]");
            await chatContext.Database.ExecuteSqlRawAsync("DELETE FROM [AspNetUserRoles]");
            await chatContext.Database.ExecuteSqlRawAsync("DELETE FROM [UserChats]");
            await chatContext.Database.ExecuteSqlRawAsync("DELETE FROM [AspNetUsers]");
            await chatContext.Database.ExecuteSqlRawAsync("DELETE FROM [AspNetUserTokens]");
            await chatContext.Database.ExecuteSqlRawAsync("DELETE FROM [Chats]");
            await chatContext.Database.ExecuteSqlRawAsync("DELETE FROM [Messages]");
        }
    }
}
