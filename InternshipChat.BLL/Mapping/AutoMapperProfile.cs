using AutoMapper;
using InternshipChat.DAL.Entities;
using InternshipChat.DAL.Helpers;
using InternshipChat.Shared.DTO;
using InternshipChat.Shared.DTO.ChatDtos;
using InternshipChat.Shared.DTO.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.BLL.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Message, MessageDTO>().ReverseMap();

            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<PagingResponseDTO<User>, PagedList<User>>().ReverseMap();

            CreateMap<User, UpdateUserDTO>().ReverseMap();

            CreateMap<Chat, ChatDTO>().ReverseMap();

            CreateMap<Chat, CreateChatDTO>().ReverseMap();
        }
    }
}
