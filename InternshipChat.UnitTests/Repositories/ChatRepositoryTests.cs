using Bogus;
using InternshipChat.DAL.Data;
using InternshipChat.DAL.Entities;
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
    public class ChatRepositoryTests
    {
        private IChatRepository _chatRepository;
        private ChatContext _chatContext;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            _chatContext = await InMemoryDatabase.GetDbContext();
            _chatRepository = new ChatRepository(_chatContext);
        }

        [Test]
        public async Task GetById_Returns_ChatWithUsers()
        {
            var chats = _chatRepository.GetAll();
            var expectedChat = chats.FirstOrDefault();

            var chat = await _chatRepository.GetChatById(expectedChat.Id);

            Assert.NotNull(chat);
            Assert.AreEqual(expectedChat.Id, chat.Id);
            Assert.GreaterOrEqual(chat.UserChats.Count, 2);
        }

        [Test]
        public async Task GetById_Returns_Null()
        {
            var chat = await _chatRepository.GetChatById(9999);

            Assert.Null(chat);
        }

        [Test]
        public async Task GetByName_Returns_Chat()
        {
            var chats = _chatRepository.GetAll();
            var expectedChat = chats.FirstOrDefault();

            var chatByName = await _chatRepository.GetChatByName(expectedChat.Name);

            Assert.NotNull(chatByName);
            Assert.AreEqual(expectedChat.Id, chatByName.Id);
        }

        [Test]
        public async Task GetByName_Returns_Null()
        {
            var chatByName = await _chatRepository.GetChatByName("non@#existing@3chat");

            Assert.Null(chatByName);
        }

        [Test]
        public async Task GetAll_ChatInfos_Returns_ChatInfos()
        {
            var chatInfos = await _chatRepository.GetAllChats();

            Assert.NotNull(chatInfos);
            Assert.That(chatInfos.All(c => c.UsersCount >= 1));
        }
    }
}
