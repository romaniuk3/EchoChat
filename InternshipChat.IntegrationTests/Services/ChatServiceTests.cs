using InternshipChat.BLL.Errors;
using InternshipChat.BLL.ServiceResult;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Data;
using InternshipChat.IntegrationTests.Helpers;
using InternshipChat.Shared.DTO.ChatDtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.IntegrationTests.Services
{
    [TestFixture]
    public class ChatServiceTests
    {
        private IChatService _chatService;
        private ChatContext _chatContext;

        [OneTimeSetUp]
        public void SetUp()
        {
            _chatService = ServicesHelper.GetChatService();
            _chatContext = ServicesHelper.GetDbContext();
        }

        [Test]
        public async Task CreateChat_Returns_ChatCreated()
        {
            await SeedDbHelper.ClearDb(_chatContext);

            var usersToAddInChat = await SeedDbHelper.SeedFakeUsers(_chatContext);
            var fakeChat = ChatDataHelper.GenerateFakeChat();
            var chatToCreate = new CreateChatDTO
            {
                Name = fakeChat.Name,
                Description = fakeChat.Description,
                UserIds = usersToAddInChat.Select(x => x.Id).ToList()
            };

            var createChatResult = await _chatService.CreateChatAsync(chatToCreate);
            var createdChat = _chatContext.Chats.FirstOrDefault();
            Assert.Multiple(() =>
            {
                Assert.That(createChatResult.IsSuccess, Is.True);
                Assert.That(createdChat, Is.Not.Null);
            });
            Assert.That(createdChat.Name, Is.EqualTo(chatToCreate.Name));
        }

        [Test]
        public async Task CreateChat_Returns_ChatExistsError()
        {
            await SeedDbHelper.ClearDb(_chatContext);

            var fakeChat = ChatDataHelper.GenerateFakeChat();
            var chatToCreate = new CreateChatDTO
            {
                Name = fakeChat.Name,
                Description = fakeChat.Description,
                UserIds = new List<int>() { 1, 2, 3 }
            };
            var expectedError = Result.Failure(DomainErrors.Chat.ChatExists).Error;

            await SeedDbHelper.AddChatToDatabase(_chatContext, fakeChat);
            var createChatResult = await _chatService.CreateChatAsync(chatToCreate);
            var allChatsWithName = _chatContext.Chats.Where(c => c.Name == fakeChat.Name).Count();

            Assert.That(createChatResult.IsFailure, Is.True);
            Assert.Multiple(() =>
            {
                Assert.That(createChatResult.Error, Is.SameAs(expectedError));
                Assert.That(allChatsWithName, Is.EqualTo(1));
            });
        }

        [Test]
        public async Task CreateChat_Returns_UserNotFoundError()
        {
            await SeedDbHelper.ClearDb(_chatContext);

            var fakeChat = ChatDataHelper.GenerateFakeChat();
            var chatToCreate = new CreateChatDTO
            {
                Name = fakeChat.Name,
                Description = fakeChat.Description,
                UserIds = new List<int> { 1, 2, 3 }
            };
            var expectedError = Result.Failure(DomainErrors.User.NotFound).Error;

            var createChatResult = await _chatService.CreateChatAsync(chatToCreate);
            var createdChat = _chatContext.Chats.FirstOrDefault(c => c.Name == fakeChat.Name);

            Assert.Multiple(() =>
            {
                Assert.That(createChatResult.IsFailure, Is.True);
                Assert.That(createChatResult.Error, Is.SameAs(expectedError));
                Assert.That(createdChat, Is.Null);
            });
        }

        [Test]
        public async Task GetAll_Returns_AllChats()
        {
            await SeedDbHelper.ClearDb(_chatContext);

            var fakeChats = await SeedDbHelper.SeedFakeChats(_chatContext);
            var chats = await _chatService.GetAllChatsAsync();

            Assert.That(chats.Count(), Is.EqualTo(fakeChats.Count));
        }

        [Test]
        public async Task GetChat_Returns_Chat()
        {
            await SeedDbHelper.ClearDb(_chatContext);

            var fakeChat = ChatDataHelper.GenerateFakeChat();
            var addedChat = await SeedDbHelper.AddChatToDatabase(_chatContext, fakeChat);

            var chatByIdResult = await _chatService.GetChatAsync(addedChat.Id);

            Assert.Multiple(() =>
            {
                Assert.That(chatByIdResult.IsSuccess, Is.True);
                Assert.That(chatByIdResult.Value, Is.Not.Null);
                Assert.That(chatByIdResult.Value.Name, Is.EqualTo(fakeChat.Name));
            });
        }

        [Test]
        public async Task GetChat_Returns_NotFoundError()
        {
            await SeedDbHelper.ClearDb(_chatContext);

            var expectedError = Result.Failure(DomainErrors.Chat.NotFound).Error;
            var chatByIdResult = await _chatService.GetChatAsync(999);

            Assert.Multiple(() =>
            {
                Assert.That(chatByIdResult.IsFailure, Is.True);
                Assert.That(chatByIdResult.Error, Is.SameAs(expectedError));
            });
        }

        [Test]
        public async Task GetAllUserChats_Returns_UserChats()
        {
            await SeedDbHelper.ClearDb(_chatContext);
            var allUserChatEntities = await SeedDbHelper.SeedFakeUserChats(_chatContext);
            var randomUser = await _chatContext.Users.FirstOrDefaultAsync();
            var expectedChatIds = allUserChatEntities.Where(uc => uc.UserId == randomUser.Id).Select(c => c.ChatId).ToList();

            var userChatsResult = await _chatService.GetUserChatsAsync(randomUser.Id);

            Assert.Multiple(() =>
            {
                Assert.That(userChatsResult.IsSuccess, Is.True);
                Assert.That(userChatsResult.Value.Count, Is.EqualTo(expectedChatIds.Count));
            });
            var actualChatIds = userChatsResult.Value.Select(c => c.Id).ToList();
            CollectionAssert.AreEquivalent(expectedChatIds, actualChatIds);
        }

        [Test]
        public async Task GetAllUserChats_Returns_UserNotFoundError()
        {
            await SeedDbHelper.ClearDb(_chatContext);

            var userChatsResult = await _chatService.GetUserChatsAsync(999);
            var expectedError = Result.Failure(DomainErrors.User.NotFound).Error;

            Assert.Multiple(() =>
            {
                Assert.That(userChatsResult.IsFailure, Is.True);
                Assert.That(userChatsResult.Error, Is.SameAs(expectedError));
            });
        }

        [Test]
        public async Task JoinToChat_Returns_Success()
        {
            await SeedDbHelper.ClearDb(_chatContext);

            var userWhoJoins = await SeedDbHelper.AddUserToDb(_chatContext);
            var chatToJoin = await SeedDbHelper.AddChatToDb(_chatContext);

            var joinResult = await _chatService.AddUserToChatAsync(chatToJoin.Id, userWhoJoins.Id);
            var userChat = await _chatContext.UserChats.FirstOrDefaultAsync(uc => uc.UserId == userWhoJoins.Id);

            Assert.That(joinResult.IsSuccess, Is.True);
            Assert.Multiple(() =>
            {
                Assert.That(userChat, Is.Not.Null);
                Assert.That(userChat.UserId, Is.EqualTo(userWhoJoins.Id));
            });
        }

        [Test]
        public async Task JoinToChat_Returns_ChatNotFoundError()
        {
            await SeedDbHelper.ClearDb(_chatContext);

            var userWhoJoins = await SeedDbHelper.AddUserToDb(_chatContext);
            int nonExistingChatId = 999;
            var expectedError = Result.Failure(DomainErrors.Chat.NotFound).Error;

            var joinResult = await _chatService.AddUserToChatAsync(nonExistingChatId, userWhoJoins.Id);

            Assert.That(joinResult.IsFailure, Is.True);
            Assert.That(joinResult.Error, Is.SameAs(expectedError));
        }

        [Test]
        public async Task JoinToChat_Returns_UserNotFoundError()
        {
            await SeedDbHelper.ClearDb(_chatContext);

            int nonExistingUserId = 999;
            var chat = await SeedDbHelper.AddChatToDb(_chatContext);
            var expectedError = Result.Failure(DomainErrors.User.NotFound).Error;

            var joinResult = await _chatService.AddUserToChatAsync(chat.Id, nonExistingUserId);

            Assert.That(joinResult.IsFailure, Is.True);
            Assert.That(joinResult.Error, Is.SameAs(expectedError));
        }
    }
}
