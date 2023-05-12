using Bogus;
using InternshipChat.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.IntegrationTests.Helpers
{
    public class MessageDataHelper
    {
        public static Message GenerateFakeMessage(int chatId, int userId)
        {
            var faker = new Faker<Message>()
                .RuleFor(m => m.MessageContent, f => f.Lorem.Word())
                .RuleFor(m => m.ChatId, chatId)
                .RuleFor(m => m.UserId, userId);

            return faker;
        }
    }
}
