using InternshipChat.BLL.Errors;
using InternshipChat.BLL.ServiceResult;
using InternshipChat.BLL.Services;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Entities;
using InternshipChat.DAL.Repositories.Interfaces;
using InternshipChat.DAL.UnitOfWork;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.UnitTests.Services
{
    [TestFixture]
    public class MessageServiceTests
    {
        private IMessageService _messageService;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMessageRepository> _mockMessageRepository;
        private Mock<IChatRepository> _mockChatRepository;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMessageRepository = new Mock<IMessageRepository>();
            _mockChatRepository = new Mock<IChatRepository>();
            _mockUnitOfWork.Setup(uow => uow.GetRepository<IMessageRepository>()).Returns(_mockMessageRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.GetRepository<IChatRepository>()).Returns(_mockChatRepository.Object);

            _messageService = new MessageService(_mockUnitOfWork.Object);
        }

        [Test]
        public void SendMessage_Returns_Message()
        {
            var message = InMemoryDatabase.GenerateFakeMessage(2, 2);

            _messageService.SendMessage(message);

            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
        }

        [Test]
        public async Task GetAll_Returns_ChatNotFoundError()
        {
            _mockChatRepository.Setup(repo => repo.GetChatById(It.IsAny<int>())).ReturnsAsync(() => null);
            var expectedResult = Result.Failure(DomainErrors.Chat.NotFound);

            var messagesResult = await _messageService.GetMessagesAsync(1);

            Assert.IsTrue(messagesResult.IsFailure);
            Assert.AreEqual(expectedResult.Error, messagesResult.Error);
        }

        [Test]
        public async Task GetAll_Returns_AllInChat()
        {
            var expectedMessaged = new List<Message>
            {
                InMemoryDatabase.GenerateFakeMessage(1, 2),
                InMemoryDatabase.GenerateFakeMessage(1, 3),
            };
            _mockMessageRepository.Setup(repo => repo.GetMessages(It.IsAny<int>())).ReturnsAsync(expectedMessaged);
            _mockChatRepository.Setup(repo => repo.GetChatById(It.IsAny<int>())).ReturnsAsync(new Chat());
            var messagesResult = await _messageService.GetMessagesAsync(1);

            Assert.IsTrue(messagesResult.IsSuccess);
            Assert.IsNotEmpty(messagesResult.Value);
        }
    }
}
