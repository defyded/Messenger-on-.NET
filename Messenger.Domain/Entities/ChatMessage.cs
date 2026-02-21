using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Domain.Entities
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ReadAt { get; set; }
        public bool Deleted { get; set; }
        public Guid ChatId { get; set; }
        public virtual Chat Chat { get; set; }
        public Guid SenderId { get; set; }
        public virtual User Sender { get; set; } = null!;
        //public List<Message> Messages { get; set; } = new();
    }
}