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

        public MessageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            return Result.Success(messagesInChat);
        }
    }
}
