using InternshipChat.DAL.Data;
using InternshipChat.DAL.Repositories;
using InternshipChat.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.UnitTests.Repositories
{
    [TestFixture]
    public class MessageRepositoryTests
    {
        private IMessageRepository _messageRepository;
        private ChatContext _chatContext;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            _chatContext = await InMemoryDatabase.GetDbContext();
            _messageRepository = new MessageRepository(_chatContext);
        }

        [Test]
        public async Task GetAll_WithoutUserIncluded_Returns_All()
        {
            var messages = await _messageRepository.GetAll().ToListAsync();

            Assert.IsNotEmpty(messages);
            Assert.That(messages.All(m => m.User == null));
        }

        [Test]
        public async Task GetAll_WithUsersIncluded_Returns_All()
        {
            var chatId = _chatContext.Chats.First().Id;
            var messages = await _messageRepository.GetMessages(chatId);

            Assert.IsNotEmpty(messages);
            Assert.That(messages.All(m => m.User != null));
        }
    }
}
