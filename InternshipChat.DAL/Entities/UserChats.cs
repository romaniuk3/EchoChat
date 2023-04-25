using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.DAL.Entities
{
    [PrimaryKey(nameof(UserId), nameof(ChatId))]
    public class UserChats
    {
        public int UserId { get; set; }
        public int ChatId { get; set; }
        public User User { get; set; }
        public Chat Chat { get; set; }
    }
}
