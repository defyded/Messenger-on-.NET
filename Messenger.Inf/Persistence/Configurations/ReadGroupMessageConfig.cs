using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Infastructure.Persistence.Configurations
{
    public class ReadGroupMessageConfig : IEntityTypeConfiguration<ReadGroupMessage>
    {
        public void Configure(EntityTypeBuilder<ReadGroupMessage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasOne(x => x.ReadMessageGroupMessage)
                .WithMany()
                .HasForeignKey(u => u.ReadMessageGroupMessageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ReadMessageUser)
                .WithMany()
                .HasForeignKey(u => u.ReadMessageUserId)
                .OnDelete(DeleteBehavior.SetNull);
        }

    }

}