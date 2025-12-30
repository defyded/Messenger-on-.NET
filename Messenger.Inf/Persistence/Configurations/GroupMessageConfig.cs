using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Infastructure.Persistence.Configurations
{
    public class GroupMessageConfig : IEntityTypeConfiguration<GroupMessage>
    {
        public void Configure(EntityTypeBuilder<GroupMessage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Content).HasMaxLength(2048);
            builder.Property(x => x.CreatedAt).IsRequired();
            
            builder.HasOne(x => x.Group)
                .WithMany(u => u.Messages)
                .HasForeignKey(x => x.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.UserFrom)
                .WithMany()
                .HasForeignKey(x => x.UserFromId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.ReadByUsers)
                .WithOne(x => x.ReadMessageGroupMessage)
                .HasForeignKey(x => x.ReadMessageGroupMessageId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(x => x.CreatedAt);
            builder.HasIndex(x => x.UserFrom);

        }
    }
}