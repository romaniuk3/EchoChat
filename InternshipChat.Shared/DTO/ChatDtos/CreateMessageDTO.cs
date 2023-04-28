using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.Shared.DTO.ChatDtos
{
    public class CreateMessageDTO
    {
        [Required]
        public int ChatId { get; set; }
        public int UserId { get; set; }
        [Required]
        public string? MessageContent { get; set; }
    }
}
