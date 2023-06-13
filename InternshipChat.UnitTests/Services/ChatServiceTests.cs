using AutoMapper;
using InternshipChat.BLL.Errors;
using InternshipChat.BLL.ServiceResult;
using InternshipChat.BLL.Services;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Entities;
using InternshipChat.DAL.Repositories.Interfaces;
using InternshipChat.DAL.UnitOfWork;
using InternshipChat.Shared.DTO.ChatDtos;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.UnitTests.Services
{
    [TestFixture]
    public class ChatServiceTests
    {
        private IChatService _chatService;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IChatRepository> _mockChatRepository;
        private Mock<IUserChatsRepository> _mockUserChatsRepository;
        private Mock<IFileService> _mockFileService;
        private Mock<IMapper> _mockMapper;

        [SetUp]
        public void SetUp()
        {
            _mockFileService = new Mock<IFileService>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockChatRepository = new Mock<IChatRepository>();
            _mockUserChatsRepository = new Mock<IUserChatsRepository>();
            _mockUnitOfWork.Setup(uow => uow.GetRepository<IUserRepository>()).Returns(_mockUserRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.GetRepository<IChatRepository>()).Returns(_mockChatRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.GetRepository<IUserChatsRepository>()).Returns(_mockUserChatsRepository.Object);
            _mockMapper = new Mock<IMapper>();
            _chatService = new ChatService(_mockUnitOfWork.Object, _mockMapper.Object, _mockFileService.Object);
        }

        [Test]
        public async Task GetAll_Returns_ChatInfoViews()
        {
            var chatsList = new List<ChatInfoView>
            {
                new ChatInfoView(),
                new ChatInfoView(),
                new ChatInfoView()
            }.AsQueryable();

            var expectedChatsList = chatsList.AsQueryable().BuildMock();
            _mockChatRepository.Setup(repo => repo.GetAllChats()).ReturnsAsync(expectedChatsList);

            var chats = await _chatService.GetAllChatsAsync();

            Assert.AreEqual(expectedChatsList.Count(), chatsList.Count());
        }

        [Test]
        public async Task Create_Returns_ChatCreated()
        {
            var createChat = new CreateChatDTO
            {
                Name = "testname",
                Description = "testdescription",
                UserIds = new List<int> { 1, 2, 3 }
            };

            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<Expression<Func<User, bool>>>())).Returns(() => InMemoryDatabase.GenerateFakeUser());

            var createResult = await _chatService.CreateChatAsync(createChat);

            Assert.IsTrue(createResult.IsSuccess);
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
        }

        [Test]
        public async Task Create_Returns_ChatExistsError()
        {
            var existChat = InMemoryDatabase.GenerateFakeChat();
            var createChat = new CreateChatDTO
            {
                Name = existChat.Name
            };

            _mockChatRepository.Setup(repo => repo.GetChatByName(It.IsAny<string>())).ReturnsAsync(existChat);
            var expectedResult = Result.Failure(DomainErrors.Chat.ChatExists);

            var createResult = await _chatService.CreateChatAsync(createChat);

            Assert.IsTrue(createResult.IsFailure);
            Assert.AreSame(expectedResult.Error, createResult.Error);
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Never);
        }

        [Test]
        public async Task GetById_Returns_Chat()
        {
            var expectedChat = InMemoryDatabase.GenerateFakeChat();
            _mockChatRepository.Setup(repo => repo.GetChatById(It.IsAny<int>())).ReturnsAsync(expectedChat);

            var chatResult = await _chatService.GetChatAsync(2);

            Assert.IsTrue(chatResult.IsSuccess);
            Assert.AreSame(expectedChat, chatResult.Value);
        }

        [Test]
        public async Task GetById_Returns_ChatNotFoundError()
        {
            _mockChatRepository.Setup(repo => repo.GetChatById(It.IsAny<int>())).ReturnsAsync(() => null);
            var expectedResult = Result.Failure(DomainErrors.Chat.NotFound);

            var chatResult = await _chatService.GetChatAsync(2);

            Assert.IsTrue(chatResult.IsFailure);
            Assert.AreSame(expectedResult.Error, chatResult.Error);
        }

        [Test]
        public async Task GetUserChats_Returns_UserChats()
        {
            var expectedUserChats = new List<Chat>
            {
                InMemoryDatabase.GenerateFakeChat(),
                InMemoryDatabase.GenerateFakeChat(),
                InMemoryDatabase.GenerateFakeChat()
            };

            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<Expression<Func<User, bool>>>())).Returns(() => new User());
            _mockUserChatsRepository.Setup(repo => repo.GetAllUserChats(It.IsAny<int>())).ReturnsAsync(expectedUserChats);
            var userChats = await _chatService.GetUserChatsAsync(2);

            Assert.IsTrue(userChats.IsSuccess);
            Assert.AreEqual(expectedUserChats.Count, userChats.Value.Count());
        }

        [Test]
        public async Task GetUserChats_Returns_UserNotFoundError()
        {
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<Expression<Func<User, bool>>>())).Returns(() => null);
            var expectedResult = Result.Failure(DomainErrors.User.NotFound);

            var userChatsResult = await _chatService.GetUserChatsAsync(2);

            Assert.IsTrue(userChatsResult.IsFailure);
            Assert.AreSame(expectedResult.Error, userChatsResult.Error);
        }

        [Test]
        public async Task AddUser_ToChat_Returns_UserAdded()
        {
            var user = InMemoryDatabase.GenerateFakeUser();
            var chat = InMemoryDatabase.GenerateFakeChat();
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);
            _mockChatRepository.Setup(repo => repo.GetById(It.IsAny<Expression<Func<Chat, bool>>>())).Returns(chat);

            var addUserResult = await _chatService.AddUserToChatAsync(chat.Id, user.Id);

            Assert.IsTrue(addUserResult.IsSuccess);
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
        }

        [Test]
        public async Task AddUser_ToChat_Returns_UserNotFoundError()
        {
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<Expression<Func<User, bool>>>())).Returns(() => null);
            var expectedResult = Result.Failure(DomainErrors.User.NotFound);

            var addUserResult = await _chatService.AddUserToChatAsync(It.IsAny<int>(), It.IsAny<int>());

            Assert.IsTrue(addUserResult.IsFailure);
            Assert.AreSame(expectedResult.Error, addUserResult.Error);
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Never);
        }

        [Test]
        public async Task AddUser_ToChat_Returns_ChatNotFoundError()
        {
            _mockChatRepository.Setup(repo => repo.GetById(It.IsAny<Expression<Func<Chat, bool>>>())).Returns(() => null);
            _mockUserRepository.Setup(repo => repo.GetById(It.IsAny<Expression<Func<User, bool>>>())).Returns(() => new User());
            var expectedResult = Result.Failure(DomainErrors.Chat.NotFound);

            var addUserResult = await _chatService.AddUserToChatAsync(It.IsAny<int>(), It.IsAny<int>());

            Assert.IsTrue(addUserResult.IsFailure);
            Assert.AreSame(expectedResult.Error, addUserResult.Error);
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Never);
        }
    }
}
