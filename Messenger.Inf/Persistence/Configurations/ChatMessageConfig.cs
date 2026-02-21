using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Infastructure.Persistence.Configurations
{
    public class ChatMessageConfig : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Content).HasMaxLength(2048);

            //builder.HasOne(x => x.Chat)
            //    .WithMany(u => u.Messages)
            //    .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.CreatedAt);
            //builder.HasIndex(x => x.Chat);
        }
    }
}