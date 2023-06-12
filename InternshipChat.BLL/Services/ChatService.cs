using AutoMapper;
using InternshipChat.BLL.Errors;
using InternshipChat.BLL.ServiceResult;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Entities;
using InternshipChat.DAL.Repositories;
using InternshipChat.DAL.Repositories.Interfaces;
using InternshipChat.DAL.UnitOfWork;
using InternshipChat.Shared.DTO.ChatDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.BLL.Services
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public ChatService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<Result> CreateChatAsync(CreateChatDTO chatDto)
        {
            var newChat = _mapper.Map<Chat>(chatDto);
            var chatRepository = _unitOfWork.GetRepository<IChatRepository>();
            var userChatsRepository = _unitOfWork.GetRepository<IUserChatsRepository>();
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();

            var existedChat = await chatRepository.GetChatByName(chatDto.Name);
            if (existedChat != null) 
            {
                return Result.Failure(DomainErrors.Chat.ChatExists);
            }

            chatRepository.Add(newChat);

            foreach (var userId in chatDto.UserIds!)
            {
                var user = userRepository.GetById(u => u.Id == userId);
                if (user != null)
                {
                    var userChat = new UserChats
                    {
                        Chat = newChat,
                        User = user
                    };
                    userChatsRepository.Add(userChat);
                } else
                {
                    return Result.Failure(DomainErrors.User.NotFound);
                }
            }
            _unitOfWork.Save();

            return Result.Success();
        }

        public async Task<IEnumerable<ChatInfoView>> GetAllChatsAsync()
        {
            var repository = _unitOfWork.GetRepository<IChatRepository>();

            return await repository.GetAllChats();
        }

        public async Task<Result<IEnumerable<ChatAttachment>>> GetChatAttachments(int chatId)
        {
            var repository = _unitOfWork.GetRepository<IChatRepository>();
            var chat = await repository.GetChatById(chatId);

            if (chat == null)
            {
                return Result.Failure<IEnumerable<ChatAttachment>>(DomainErrors.Chat.NotFound);
            }
            var chatAttachments = await repository.GetAllChatAttachments(chatId);

            return Result.Success(chatAttachments);
        }

        public async Task<Result<IEnumerable<ChatAttachment>>> GetUserSignatureAttachments(int chatId, int userId)
        {
            var chatRepository = _unitOfWork.GetRepository<IChatRepository>();
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();
            var chat = await chatRepository.GetChatById(chatId);

            if (chat == null)
            {
                return Result.Failure<IEnumerable<ChatAttachment>>(DomainErrors.Chat.NotFound);
            }
            var user = userRepository.GetById(u => u.Id == userId);

            if (user == null)
            {
                return Result.Failure<IEnumerable<ChatAttachment>>(DomainErrors.User.NotFound);
            }

            var signatureAttachments = await chatRepository.GetUserSignatureAttachments(chatId, userId);
            //var attachmentsWithSasTokens = AppendSasTokenToAttachment(signatureAttachments);

            return Result.Success(signatureAttachments);
        }

        public IEnumerable<ChatAttachment> AppendSasTokenToAttachment(IEnumerable<ChatAttachment> attachments)
        {
            var containerName = "attachments-container";

            foreach (var attachment in attachments)
            {
                var sasToken = _fileService.GenerateSasTokenForBlob(attachment.FileName, containerName);
                attachment.AttachmentUrl = attachment.AttachmentUrl != null ? $"{attachment.AttachmentUrl}?{sasToken}" : attachment.AttachmentUrl!;
            }

            return attachments;
        }

        public async Task<Result<IEnumerable<Chat>>> GetUserChatsAsync(int userId)
        {
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();
            var user = userRepository.GetById(u => u.Id == userId);
            if (user == null)
            {
                return Result.Failure<IEnumerable<Chat>>(DomainErrors.User.NotFound);
            }

            var userChatsRepository = _unitOfWork.GetRepository<IUserChatsRepository>();
            var userChats = await userChatsRepository.GetAllUserChats(userId);

            return Result.Success(userChats);
        }

        public async Task<Result<Chat>> GetChatAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<IChatRepository>();
            var chat = await repository.GetChatById(id);
            if (chat == null)
            {
                return Result.Failure<Chat>(DomainErrors.Chat.NotFound);
            }

            return chat;
        }

        public async Task<Result<ChatAttachment>> AddChatAttachment(ChatAttachment chatAttachment)
        {
            var repository = _unitOfWork.GetRepository<IChatRepository>();
            await repository.SaveAttachment(chatAttachment);
            _unitOfWork.Save();

            return Result.Success(chatAttachment);
        }

        public async Task<Result> UpdateAttachment(int attachmentId, ChatAttachment newAttachment)
        {
            var repository = _unitOfWork.GetRepository<IChatRepository>();
            var chatAttachment = await repository.GetChatAttachment(attachmentId);

            if (chatAttachment == null )
            {
                return Result.Failure(DomainErrors.Chat.NotFound);
            }

            chatAttachment.AttachmentUrl = newAttachment.AttachmentUrl;
            chatAttachment.RequiresSignature = newAttachment.RequiresSignature;
            _unitOfWork.Save();

            return Result.Success();
        }

        public async Task<Result> AddUserToChatAsync(int chatId, int userId)
        {
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();
            var chatRepository = _unitOfWork.GetRepository<IChatRepository>();
            var userChatsRepository = _unitOfWork.GetRepository<IUserChatsRepository>();

            var user = userRepository.GetById(u => u.Id == userId);
            if (user == null)
            {
                return Result.Failure<User>(DomainErrors.User.NotFound);
            }
            var chat = chatRepository.GetById(c => c.Id == chatId);
            if (chat == null)
            {
                return Result.Failure<Chat>(DomainErrors.Chat.NotFound);
            }

            var userChat = new UserChats
            {
                Chat = chat,
                User = user
            };
            userChatsRepository.Add(userChat);

            _unitOfWork.Save();

            return Result.Success();
        }
    }
}
