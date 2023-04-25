using AutoMapper;
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

        public async void CreateChat(ChatDTO chatDto)
        {
            var chat = _mapper.Map<Chat>(chatDto);
            var chatRepository = _unitOfWork.GetRepository<IChatRepository>();
            var userChatsRepository = _unitOfWork.GetRepository<IUserChatsRepository>();
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();

            chatRepository.Add(chat);

            foreach (var userId in chatDto.UserIds)
            {
                var user = userRepository.GetById(u => u.Id == userId);
                if (user != null)
                {
                    var userChat = new UserChats
                    {
                        Chat = chat,
                        User = user
                    };
                    userChatsRepository.Add(userChat);
                }
            }
            await _unitOfWork.SaveAsync();
        }

        public IEnumerable<Chat> GetAllChats()
        {
            var repository = _unitOfWork.GetRepository<IChatRepository>();
            return repository.GetAll();
        }
    }
}
