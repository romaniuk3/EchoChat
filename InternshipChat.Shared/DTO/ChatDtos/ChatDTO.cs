﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.Shared.DTO.ChatDtos
{
    public class ChatDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> UserIds { get; set; }
        public IEnumerable<UserDTO> Users { get; set; }
        public int UsersCount { get; set; }
    }
}
