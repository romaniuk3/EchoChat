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
        public void CreateChat_Should_AddChat_ToDb()
        {
            
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            _chatContext.Database.EnsureDeleted();
        }
    }
}
