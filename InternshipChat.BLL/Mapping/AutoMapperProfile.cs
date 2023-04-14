using AutoMapper;
using InternshipChat.BLL.DTO;
using InternshipChat.DAL.Entities;
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
        }
    }
}
