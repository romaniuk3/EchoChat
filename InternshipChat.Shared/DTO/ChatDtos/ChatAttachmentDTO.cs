using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.Shared.DTO.ChatDtos
{
    public class ChatAttachmentDTO
    {
        public int SenderId { get; set; }
        public int ChatId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileText { get; set; } = string.Empty;
        public IBrowserFile Document { get; set; }
    }
}
