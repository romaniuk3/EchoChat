using InternshipChat.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.DAL.Data
{
    public class ChatContext : DbContext
    {
        public ChatContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<User> User { get; set; }
        public DbSet<Message> Message { get; set; }

    }
}
