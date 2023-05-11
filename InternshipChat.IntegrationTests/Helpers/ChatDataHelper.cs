using Bogus;
using InternshipChat.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.IntegrationTests.Helpers
{
    public class ChatDataHelper
    {
        public static Chat GenerateFakeChat()
        {
            var faker = new Faker<Chat>()
                .RuleFor(u => u.Name, f => f.Lorem.Word())
                .RuleFor(u => u.Description, f => f.Lorem.Word());

            return faker;
        }
    }
}
