using AutoMapper;
using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Entities;
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

        public void CreateChat(ChatDTO chatDto)
        {
            var chat = _mapper.Map<Chat>(chatDto);
            _unitOfWork.ChatRepository.Add(chat);
            _unitOfWork.Save();
        }

        public IEnumerable<Chat> GetAllChats()
        {
            return _unitOfWork.ChatRepository.GetAll();
        }
    }
}
