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

        public ChatService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
