using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.Infastructure.Persistence.Configurations
{
    public class UserDeviceConfig : IEntityTypeConfiguration<UserDevice>
    {
        public void Configure(EntityTypeBuilder<UserDevice> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.DeviceName).IsRequired();
            builder.Property(x => x.IpAddress).IsRequired();
            builder.Property(x => x.AddedAt).IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(u => u.UserDevices)
                .HasForeignKey(x => x.UserId);

            builder.HasIndex(x => x.DeviceName);
            builder.HasIndex(x => x.IpAddress);
            builder.HasIndex(x => x.AddedAt);
        }
    }
}