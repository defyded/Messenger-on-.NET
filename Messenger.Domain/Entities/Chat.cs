using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Domain.Entities
{
    public class Chat
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid UserIdTo { get; set; }
        public Guid UserIdFrom { get; set; }
        public virtual User UserTo { get; set; }
        public virtual User UserFrom { get; set; }
        public bool Blocked { get; set; }
        public List<ChatMessage> Messages { get; set; } = new();
    }
}