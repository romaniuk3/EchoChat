using InternshipChat.BLL.Errors;
using InternshipChat.BLL.ServiceResult;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Entities;
using InternshipChat.DAL.Repositories.Interfaces;
using InternshipChat.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.BLL.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public MessageService(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }
        public Message SendMessage(Message message)
        {
            var repository = _unitOfWork.GetRepository<IMessageRepository>();

            repository.Add(message);

            _unitOfWork.Save();
            return message;
        }

        public async Task<Result<IEnumerable<Message>>> GetMessagesAsync(int chatId)
        {
            var repository = _unitOfWork.GetRepository<IMessageRepository>();
            var chatRepository = _unitOfWork.GetRepository<IChatRepository>();
            var chat = await chatRepository.GetChatById(chatId);
            if (chat == null)
            {
                return Result.Failure<IEnumerable<Message>>(DomainErrors.Chat.NotFound);
            }

            var messagesInChat = await repository.GetMessages(chatId);
            var messagesWithUserAvatars = AppendSasTokenToUsersAvatar(messagesInChat);

            return Result.Success(messagesWithUserAvatars);
        }

        private IEnumerable<Message> AppendSasTokenToUsersAvatar(IEnumerable<Message> messages)
        {
            var sasToken = _fileService.GenerateSasTokenForBlobContainer();

            foreach (var message in messages)
            {
                var user = message.User;
                user!.Avatar = $"{user.Avatar}?{sasToken}" ?? user.Avatar;
            }

            return messages;
        }
    }
}
