using InternshipChat.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace InternshipChat.DAL.Data
{
    public class ChatContext : IdentityDbContext<User, ApplicationRole, int>
    {
        public ChatContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<User> AspNetUsers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<UserChats> UserChats { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().
                HasMany(x => x.UserChats)
                .WithOne(y => y.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Chat>()
                .HasMany(x => x.UserChats)
                .WithOne(y => y.Chat)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
                .Property(m => m.CreatedDate).HasColumnType("datetime");

            base.OnModelCreating(builder);
        }
    }
}
