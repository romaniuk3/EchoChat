using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.Shared.DTO.ChatDtos
{
    public class ChatDTO
    {
        public string Name { get; set; }
        public List<int>? UserIds { get; set; }
    }
}
