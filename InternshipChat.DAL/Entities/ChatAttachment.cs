using InternshipChat.Shared.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.DAL.Entities
{
    public class ChatAttachment
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ChatId { get; set; }
        public FileModel Attachment { get; set; }
        public bool RequiresSignature { get; set; }
        public int ReceiverId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileText { get; set; } = string.Empty;
    }
}
