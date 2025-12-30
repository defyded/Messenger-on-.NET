using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Infastructure.Persistence.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Username).IsRequired().HasMaxLength(255);
            builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(255);
            builder.Property(x => x.AvatarUrl).HasMaxLength(3100);
            builder.Property(x => x.Descrption).HasMaxLength(255);
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.LastSeenAt).IsRequired();

            //builder.HasMany(x => x.Chats)
            //    .WithOne(u => u.UserFrom)
            //    .HasForeignKey(u => u.UserIdFrom);
            
            //builder.HasMany(x => x.Chats)
            //    .WithOne(u => u.UserTo)
            //    .HasForeignKey(u => u.UserIdTo);

            builder.HasMany(x => x.Groups)
                .WithMany(u => u.Users)
                .UsingEntity<Dictionary<string, object>>(
                "users_groups",
                r => r
                    .HasOne<Group>()
                    .WithMany()
                    .HasForeignKey("group_id")
                    .OnDelete(DeleteBehavior.Cascade),
                    
                l => l
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("user_id")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("user_id", "group_id");
                    j.ToTable("users_groups");
                });

            builder.HasMany(x => x.UserDevices)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId);

            builder.HasIndex(x => x.Username);
            builder.HasIndex(x => x.Email);
            builder.HasIndex(x => x.CreatedAt);
            builder.HasIndex(x => x.LastSeenAt);
        }
    }
}