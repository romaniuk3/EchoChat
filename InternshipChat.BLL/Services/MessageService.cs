using InternshipChat.BLL.Services.Contracts;
using InternshipChat.DAL.Entities;
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
        public bool SendMessage(Message message)
        {
            _unitOfWork.MessageRepository.Add(message);

            return true;
        }
    }
}
