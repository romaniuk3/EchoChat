using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.DAL.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public int ChatId { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public string? MessageContent { get; set; }
    }
}
