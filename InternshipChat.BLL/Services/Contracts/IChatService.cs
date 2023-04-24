using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO.ChatDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.BLL.Services.Contracts
{
    public interface IChatService
    {
        void CreateChat(ChatDTO chat);
    }
}
