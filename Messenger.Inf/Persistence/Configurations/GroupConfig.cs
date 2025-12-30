using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Xml.Linq;

namespace Messenger.Infastructure.Persistence.Configurations
{
    public class GroupConfig : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
            builder.Property(x => x.AvatarUrl).HasMaxLength(3100);
            builder.Property(x => x.CreatedAt).IsRequired();

            builder.HasOne(x => x.UserOwner)
                .WithMany()
                .HasForeignKey(i => i.UserOwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Users)
                .WithMany(u => u.Groups)
                .UsingEntity<Dictionary<string, object>>(
                "users_groups",
                l => l
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("user_id")
                    .OnDelete(DeleteBehavior.Cascade),
                r => r
                    .HasOne<Group>()
                    .WithMany()
                    .HasForeignKey("group_id")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("user_id", "group_id");
                    j.ToTable("users_groups");
                });

            builder.HasMany(u => u.Messages)
                .WithOne(x => x.Group)
                .HasForeignKey(x => x.GroupId);

            builder.HasIndex(x => x.Title);
            builder.HasIndex(x => x.UserOwner);
            builder.HasIndex(x => x.CreatedAt);
        }
    }
}