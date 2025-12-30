using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Infastructure.Persistence
{
    public class MessengerDBContext : DbContext
    {
        public MessengerDBContext(DbContextOptions<MessengerDBContext> options) : base(options)
        {
            //
        }
        public DbSet<Domain.Entities.User> Users { get; set; }
        public DbSet<Domain.Entities.Chat> Chats { get; set; }
        public DbSet<Domain.Entities.ChatMessage> ChatMessages { get; set; }
        public DbSet<Domain.Entities.Group> Groups { get; set; }
        public DbSet<Domain.Entities.GroupMessage> GroupMessages { get; set; }
        public DbSet<Domain.Entities.ReadGroupMessage> ReadGroupMessages { get; set; }
        public DbSet<Domain.Entities.UserDevice> UserDevices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MessengerDBContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        //public override OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // modelBuilder.Entity<Entities.Models.ReadGroupMessage>()
        //    //     .HasKey(rgm => new { rgm.ReadMessageUserId, rgm.ReadMessageGroupMessageId });

        //    // modelBuilder.Entity<Entities.Models.ReadGroupMessage>()
        //    //     .HasOne(rgm => rgm.ReadMessageUser)
        //    //     .WithMany(u => u.ReadGroupMessages)
        //    //     .HasForeignKey(rgm => rgm.ReadMessageUserId);

        //    // modelBuilder.Entity<Entities.Models.ReadGroupMessage>()
        //    //     .HasOne(rgm => rgm.ReadMessageGroupMessage)
        //    //     .WithMany(gm => gm.ReadByUsers)
        //    //     .HasForeignKey(rgm => rgm.ReadMessageGroupMessageId);
        //    modelBuilder.ApplyConfigurationsFromAssembly(typeof(MessengerDBContext).Assembly);
        //    base.OnModelCreating(modelBuilder);
        //}

    }
}