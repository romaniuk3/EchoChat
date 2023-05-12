using InternshipChat.BLL.Errors;
using InternshipChat.BLL.ServiceResult;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Data;
using InternshipChat.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.IntegrationTests.Services
{
    [TestFixture]
    public class MessageServiceTests
    {
        private IMessageService _messageService;
        private ChatContext _chatContext;

        [OneTimeSetUp]
        public void SetUp()
        {
            _messageService = ServicesHelper.GetMessageService();
            _chatContext = ServicesHelper.GetDbContext();
        }

        [Test]
        public async Task SendMessage_Returns_Message()
        {
            await SeedDbHelper.ClearDb(_chatContext);

            var chat = await SeedDbHelper.AddChatToDb(_chatContext);
            var user = await SeedDbHelper.AddUserToDb(_chatContext);
            var fakeMessage = MessageDataHelper.GenerateFakeMessage(chat.Id, user.Id);

            _messageService.SendMessage(fakeMessage);
            var sentMessage = await _chatContext.Messages.FirstOrDefaultAsync();

            Assert.That(sentMessage, Is.Not.Null);
            Assert.That(sentMessage.UserId, Is.EqualTo(fakeMessage.UserId));
            Assert.That(sentMessage.ChatId, Is.EqualTo(fakeMessage.ChatId));
            Assert.That(sentMessage.MessageContent, Is.EqualTo(fakeMessage.MessageContent));
        }

        [Test]
        public async Task GetAll_Returns_AllMessages()
        {
            await SeedDbHelper.ClearDb(_chatContext);

            var chat = await SeedDbHelper.AddChatToDb(_chatContext);
            var user = await SeedDbHelper.AddUserToDb(_chatContext);
            var messages = await SeedDbHelper.SeedFakeMessages(_chatContext, user.Id, chat.Id);

            var messagesResult = await _messageService.GetMessagesAsync(chat.Id);

            Assert.That(messagesResult.IsSuccess, Is.True);
            Assert.That(messagesResult.Value.Count(), Is.EqualTo(messages.Count));
        }

        [Test]
        public async Task GetAll_Returns_ChatNotFoundError()
        {
            await SeedDbHelper.ClearDb(_chatContext);

            int nonExistingChatId = 999;
            var expectedError = Result.Failure(DomainErrors.Chat.NotFound).Error;

            var messagesResult = await _messageService.GetMessagesAsync(nonExistingChatId);

            Assert.That(messagesResult.IsFailure, Is.True);
            Assert.That(messagesResult.Error, Is.SameAs(expectedError));
        }
    }
}
