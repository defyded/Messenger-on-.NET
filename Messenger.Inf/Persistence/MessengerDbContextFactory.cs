using Messenger.Infastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Infastucture.Persistence
{
    public sealed class MessengerDbContextFactory : IDesignTimeDbContextFactory<MessengerDBContext>
    {
        public MessengerDBContext CreateDbContext(string[] args)
        {
            // только для генерации миграций
            var cs =
                "Host=localhost;Port=5432;Database=MessengerDB;Username=thelowestuser_messenger;Password=123456789;Pooling=false;Ssl Mode=Disable";

            var options = new DbContextOptionsBuilder<MessengerDBContext>()
                .UseNpgsql(cs)
                .Options;

            return new MessengerDBContext(options);
        }
    }
}
