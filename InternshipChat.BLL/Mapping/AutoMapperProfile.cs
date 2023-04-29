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
            
            CreateMap<Message, CreateMessageDTO>().ReverseMap();

            CreateMap<User, RegisterUserDTO>().ReverseMap();

            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<PagingResponseDTO<User>, PagedList<User>>().ReverseMap();
            CreateMap<PagedList<User>, PagingResponseDTO<UserDTO>>()
                .ForMember(
                    destination => destination.Items,
                    options => options.MapFrom(src => src)
                );

            CreateMap<User, UpdateUserDTO>().ReverseMap();

            CreateMap<Chat, ChatDTO>()
                .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.UserChats.Select(uc => uc.User)));

            CreateMap<Chat, CreateChatDTO>().ReverseMap();
        }
    }
}
