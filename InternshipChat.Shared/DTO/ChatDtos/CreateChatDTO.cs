using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.Shared.DTO.ChatDtos
{
    public class CreateChatDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int>? UserIds { get; set; }
    }
}
