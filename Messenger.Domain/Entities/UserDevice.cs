using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Domain.Entities
{
    public  class UserDevice
    {
        public Guid Id { get; set; }
        public string DeviceName { get; set; }
        public IPAddress IpAddress { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastActivity { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}