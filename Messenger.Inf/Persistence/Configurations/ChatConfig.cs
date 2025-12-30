using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infastructure.Persistence.Configurations
{
    public class ChatConfig : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CreatedAt).IsRequired();

            //builder.HasOne(x => x.UserTo);
                //.WithMany()
                //.HasForeignKey(x => x.UserIdTo)
                //.OnDelete(DeleteBehavior.Restrict);


            //builder.HasOne(x => x.UserFrom)
            //    .WithMany()
            //    .HasForeignKey(i => i.UserIdFrom)
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Messages)
                .WithOne(m => m.Chat)
                .HasForeignKey(m => m.ChatId);

            //builder.HasIndex(x => x.UserTo);
            //builder.HasIndex(x => x.UserFrom);
            builder.HasIndex(x => x.CreatedAt);
            //builder.HasIndex(x => x.Blocked);
        }
    }
}